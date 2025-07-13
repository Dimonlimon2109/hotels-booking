using HotelsBooking.BLL.DTO;
using HotelsBooking.BLL.Models;

namespace HotelsBooking.BLL.Interfaces
{
    public interface IBookingService
    {
        Task<string> CreateBookingAsync(CreateBookingDTO creatingBooking, string userEmail, CancellationToken ct = default);
        Task<BookingDTO> GetBookingByIdAsync(int id, CancellationToken ct = default);
        Task<IEnumerable<BookingDTO>> GetUserBookingsAsync(string userEmail, CancellationToken ct = default);
        Task<bool> IsRoomAvailableAsync(int roomId, BookingDatesModel bookingDates, CancellationToken ct = default);
        Task ConfirmBookingAsync(string json, string signature, CancellationToken ct = default);
        Task CancelUnpaidBookingAsync(int bookingId, CancellationToken ct = default);
        Task CancelBookingAsync(int bookingId, string cancellationReason, CancellationToken ct = default);
        Task<IEnumerable<BookingDTO>> GetBookingsByRoomAsync(int roomId, string userEmail, CancellationToken ct = default);
    }
}