using AutoMapper;
using HotelsBooking.API.Constants;
using HotelsBooking.API.Models;
using HotelsBooking.API.ViewModels;
using HotelsBooking.BLL.DTO;
using HotelsBooking.BLL.Interfaces;
using HotelsBooking.BLL.Models;
using HotelsBooking.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotelsBooking.API.Controllers
{
    [Route("api/bookings")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IStripeService _stripeSerice;
        private readonly IMapper _mapper;

        public BookingController(IBookingService bookingService, IMapper mapper, IStripeService stripeSerice)
        {
            _bookingService = bookingService;
            _mapper = mapper;
            _stripeSerice = stripeSerice;
        }

        [Authorize(Policy = Policies.Client)]
        [HttpPost]
        public async Task<IActionResult> Create(CreateBookingModel creatingBooking, CancellationToken ct = default)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var creatingBookingDTO = _mapper.Map<CreateBookingDTO>(creatingBooking);

            var checkoutUrl = await _bookingService.CreateBookingAsync(creatingBookingDTO, userEmail, ct);
            return Ok(checkoutUrl);
        }

        [Authorize]
        [HttpGet("users/{userId:int}")]
        public async Task<IActionResult> GetAllByUserId(int userId, CancellationToken ct = default)
        {
            var bookingsDTO = await _bookingService.GetUserBookingsAsync(userId, ct);
            return Ok(bookingsDTO.Select(bd => _mapper.Map<BookingViewModel>(bd)));
        }

        [Authorize]
        [HttpGet("{bookingId:int}")]
        public async Task<IActionResult> GetById(int bookingId, CancellationToken ct = default)
        {
            var bookingDTO = await _bookingService.GetBookingByIdAsync(bookingId, ct);
            return Ok(_mapper.Map<BookingViewModel>(bookingDTO));
        }

        [Authorize(Roles = $"{Roles.HotelOwner},{Roles.Client}")]
        [HttpPut("{bookingId:int}")]
        public async Task<IActionResult> UpdateBookingStatus(int bookingId, UpdateBookingStatusModel status, CancellationToken ct = default)
        {
            var updatingStatusDTO = _mapper.Map<UpdateBookingStatusDTO>(status);

            await _bookingService.UpdateBookingStatusAsync(bookingId, updatingStatusDTO, ct);

            return Ok();
        }

        [HttpPost("rooms/{roomId:int}/availability")]
        public async Task<IActionResult> CheckAvailability(
            int roomId,
            BookingDatesModel bookingDates,
            CancellationToken ct)
        {
            var isAvailable = await _bookingService.IsRoomAvailableAsync(roomId, bookingDates, ct);
            return Ok(new { isAvailable });
        }

        [HttpPost("payment/webhook")]
        public async Task<IActionResult> BookingPaymentWebook(CancellationToken ct = default)
        {
            using var reader = new StreamReader(Request.Body);
            var json = await reader.ReadToEndAsync();
            var signature = Request.Headers["Stripe-Signature"];

            await _bookingService.ConfirmBookingAsync(json, signature, ct);

            return Ok();
        }
    }
}
