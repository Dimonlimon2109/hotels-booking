
using AutoMapper;
using FluentValidation;
using HotelsBooking.BLL.DTO;
using HotelsBooking.BLL.Models;
using HotelsBooking.DAL.Constants;
using HotelsBooking.DAL.Entities;
using HotelsBooking.DAL.Interfaces;
using System.Security;

namespace HotelsBooking.BLL.Services
{
    public class BookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateBookingDTO> _creatingBookingValidator;
        private readonly IValidator<UpdateBookingStatusDTO> _updatingBookingStatusValidator;
        private readonly PdfGenerator _pdfGenerator;
        private readonly SmtpEmailSender _emailSender;

        public BookingService(
            IBookingRepository bookingRepository,
            IRoomRepository roomRepository,
            IUserRepository userRepository,
            IMapper mapper,
            IValidator<CreateBookingDTO> creatingBookingValidator,
            IValidator<UpdateBookingStatusDTO> updatingBookingStatusValidator,
            PdfGenerator pdfGenerator,
            SmtpEmailSender emailSender
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
        }

        public async Task CreateBookingAsync(CreateBookingDTO creatingBooking, string userEmail, CancellationToken ct = default)
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

            if(bookingRoom.Capacity != creatingBooking.Adults + creatingBooking.Children)
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
            await _bookingRepository.AddAsync(booking);
            await _bookingRepository.SaveChangesAsync(ct);
        }

        public async Task<IEnumerable<BookingDTO>> GetUserBookingsAsync(int userId, CancellationToken ct = default)
        {
            var user = await _userRepository.GetByIdAsync(userId, ct)
                ?? throw new SecurityException("Пользователь не найден");

            var userBookings = await _bookingRepository.GetBookingsByUserIdAsync(userId, ct);

            return userBookings.Select(ub => _mapper.Map<BookingDTO>(ub));
        }
        public async Task<BookingDTO> GetBookingByIdAsync(int id, CancellationToken ct = default)
        {
            var booking = await _bookingRepository.GetByIdAsync(id, ct)
                ?? throw new NullReferenceException("Бронирование не найдено");

            return _mapper.Map<BookingDTO>(booking);
        }

        public async Task UpdateBookingStatusAsync(int id,
            UpdateBookingStatusDTO updatingBookingStatus,
            CancellationToken ct = default)
        {
            var validationResult = _updatingBookingStatusValidator.Validate(updatingBookingStatus);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            if(!Enum.TryParse<BookingStatus>(updatingBookingStatus.Status, out var status))
            {
                throw new ArgumentException("Некорректный статус бронирования");
            }

            var booking = await _bookingRepository.GetByIdAsync(id, ct)
                ?? throw new NullReferenceException("Бронирование не найдено");

            var allowedTransitions = new Dictionary<BookingStatus, BookingStatus[]>
            {
        { BookingStatus.Pending, new[] { BookingStatus.Confirmed, BookingStatus.Cancelled } },
        { BookingStatus.Confirmed, new[] { BookingStatus.CheckedIn, BookingStatus.Cancelled } },
        { BookingStatus.CheckedIn, new[] { BookingStatus.CheckedOut } },
        { BookingStatus.CheckedOut, Array.Empty<BookingStatus>() },
        { BookingStatus.Cancelled, Array.Empty<BookingStatus>() }
            };

            if (!allowedTransitions.TryGetValue(booking.Status, out var validStatuses) ||
                !validStatuses.ToList().Contains(status))
            {
                throw new InvalidOperationException(
                    $"Переход из статуса {booking.Status.ToString()} в {updatingBookingStatus.Status} недопустим.");
            }

            booking.Status = status;

            _bookingRepository.Update(booking);
            await _bookingRepository.SaveChangesAsync(ct);

            if(booking.Status == BookingStatus.Confirmed)
            {
                await NotifyUserAsync(booking);
            }
        }

        public async Task<bool> IsRoomAvailableAsync(int roomId, BookingDatesModel bookingDates, CancellationToken ct = default)
        {
            var room = await _roomRepository.GetByIdAsync(roomId)
                ?? throw new NullReferenceException("Номер отеля не найден");

            return await _bookingRepository.IsRoomAvailableAsync(roomId, bookingDates.CheckInDate, bookingDates.CheckOutDate, ct);
        }

        public async Task NotifyUserAsync(Booking booking, CancellationToken ct = default)
        {
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
    }
}
