
using AutoMapper;
using FluentValidation;
using Hangfire;
using HotelsBooking.BLL.DTO;
using HotelsBooking.BLL.Interfaces;
using HotelsBooking.BLL.Models;
using HotelsBooking.DAL.Constants;
using HotelsBooking.DAL.Entities;
using HotelsBooking.DAL.Interfaces;
using System.Security;
using System.Text.Json.Nodes;

namespace HotelsBooking.BLL.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateBookingDTO> _creatingBookingValidator;
        private readonly IValidator<UpdateBookingStatusDTO> _updatingBookingStatusValidator;
        private readonly IPdfGenerator _pdfGenerator;
        private readonly ISmtpEmailSender _emailSender;
        private readonly IStripeService _stripeService;

        public BookingService(
            IBookingRepository bookingRepository,
            IRoomRepository roomRepository,
            IUserRepository userRepository,
            IMapper mapper,
            IValidator<CreateBookingDTO> creatingBookingValidator,
            IValidator<UpdateBookingStatusDTO> updatingBookingStatusValidator,
            IPdfGenerator pdfGenerator,
            ISmtpEmailSender emailSender,
            IStripeService stripeService
            )
        {
            _bookingRepository = bookingRepository;
            _roomRepository = roomRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _creatingBookingValidator = creatingBookingValidator;
            _updatingBookingStatusValidator = updatingBookingStatusValidator;
            _pdfGenerator = pdfGenerator;
            _emailSender = emailSender;
            _stripeService = stripeService;
        }

        public async Task<string> CreateBookingAsync(CreateBookingDTO creatingBooking, string userEmail, CancellationToken ct = default)
        {
            var validationResult = _creatingBookingValidator.Validate(creatingBooking);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var bookingRoom = await _roomRepository.GetByIdAsync(creatingBooking.RoomId, ct)
                ?? throw new NullReferenceException("Номер отеля не найден");

            var user = await _userRepository.GetByEmailAsync(userEmail, ct)
                ?? throw new SecurityException("Пользователь не найден");

            if (bookingRoom.Capacity != creatingBooking.Adults + creatingBooking.Children)
            {
                throw new ArgumentException("Количество взрослых и детей должно быть равно количеству мест в номере");
            }

            var overlappingBookingsCount = await _bookingRepository.GetOverlappingBookingsCountAsync(
                creatingBooking.RoomId,
                creatingBooking.CheckInDate,
                creatingBooking.CheckOutDate,
                ct
            );

            if (overlappingBookingsCount != 0)
            {
                throw new InvalidOperationException("Невозможно создать бронь: указанные даты уже заняты");
            }

            var booking = _mapper.Map<Booking>(creatingBooking);
            booking.UserId = user.Id;
            booking.TotalPrice = bookingRoom.PricePerNight * booking.Adults + bookingRoom.PricePerNight * booking.Children;

            var jobId = BackgroundJob.Schedule<IBookingService>(
                x => x.CancelUnpaidBookingAsync(booking.Id, CancellationToken.None),
                TimeSpan.FromMinutes(30));

            booking.CancellationJobId = jobId;
            await _bookingRepository.AddAsync(booking);
            await _bookingRepository.SaveChangesAsync(ct);

            var checkoutSession = await _stripeService.CreateBookingCheckoutSessionAsync(
                user.Email,
                booking.TotalPrice,
                booking.Id.ToString(),
                ct);

            return checkoutSession.Url;
        }

        public async Task<IEnumerable<BookingDTO>> GetUserBookingsAsync(string userEmail, CancellationToken ct = default)
        {
            var user = await _userRepository.GetByEmailAsync(userEmail, ct)
                ?? throw new SecurityException("Пользователь не найден");

            var userBookings = await _bookingRepository.GetBookingsByUserIdAsync(user.Id, ct);

            return userBookings.Select(ub => _mapper.Map<BookingDTO>(ub));
        }
        public async Task<BookingDTO> GetBookingByIdAsync(int id, CancellationToken ct = default)
        {
            var booking = await _bookingRepository.GetByIdAsync(id, ct)
                ?? throw new NullReferenceException("Бронирование не найдено");

            return _mapper.Map<BookingDTO>(booking);
        }

        public async Task CancelBookingAsync(int bookingId, string cancellationReason, CancellationToken ct = default)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId, ct)
                ?? throw new NullReferenceException("Бронирование не найдено");

            if (booking.Status == BookingStatus.Confirmed)
            {
                if (DateTime.UtcNow >= booking.CheckInDate)
                    throw new InvalidOperationException("Нельзя отменить бронирование после даты заезда");

                await _stripeService.RefundPaymentAsync(booking.ChargeId, ct);
            }

            booking.CancellationReason = cancellationReason;
            booking.Status = BookingStatus.Cancelled;
            _bookingRepository.Update(booking);
            await _bookingRepository.SaveChangesAsync(ct);
        }

        public async Task<bool> IsRoomAvailableAsync(int roomId, BookingDatesModel bookingDates, CancellationToken ct = default)
        {
            var room = await _roomRepository.GetByIdAsync(roomId)
                ?? throw new NullReferenceException("Номер отеля не найден");

            return await _bookingRepository.IsRoomAvailableAsync(roomId, bookingDates.CheckInDate, bookingDates.CheckOutDate, ct);
        }

        public async Task ConfirmBookingAsync(string json, string signature, CancellationToken ct = default)
        {

            var paymentInfo = await _stripeService.HandleBookingWebhook(json, signature);

            var booking = await _bookingRepository.GetByIdAsync(paymentInfo.BookingId, ct)
                ?? throw new NullReferenceException("Бронирование не найдено");

            if (!string.IsNullOrEmpty(booking.CancellationJobId))
            {
                BackgroundJob.Delete(booking.CancellationJobId);
            }

            booking.CancellationJobId = null;
            booking.ChargeId = paymentInfo.ChargeId;
            booking.Status = BookingStatus.Confirmed;
            _bookingRepository.Update(booking);
            await _bookingRepository.SaveChangesAsync(ct);

            var message = $@"<p>Здравствуйте, {booking.User?.UserName}!</p>
                             <p>Вы успешно забронировали номер в отеле.</p>
                             <p>Период: <b>{booking.CheckInDate:dd.MM.yyyy} – {booking.CheckOutDate:dd.MM.yyyy}</b></p>
                             <p>Общая стоимость: <b>{booking.TotalPrice} BYN</b></p>
                             <p>PDF-файл подтверждения бронирования прикреплён к письму.</p>";

            var pdfAttachment = _pdfGenerator.GenerateBookingConfirmation(booking);

            await _emailSender.SendEmailWithPdfAsync(
                booking.User.Email,
                $"Подтверждение бронирования №{booking.Id}",
                message,
                $"Booking_{booking.Id}.pdf",
                pdfAttachment,
                ct
            );
        }

        public async Task CancelUnpaidBookingAsync(int bookingId, CancellationToken ct = default)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId, ct)
                ?? throw new Exception("Бронирование не найдено");
            if (booking.Status != BookingStatus.Pending || booking.Status == BookingStatus.Cancelled)
                return;

            booking.Status = BookingStatus.Cancelled;
            _bookingRepository.Update(booking);
            await _bookingRepository.SaveChangesAsync(ct);
        }
    }
}
