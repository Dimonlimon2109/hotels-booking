using HotelsBooking.BLL.DTO;
using HotelsBooking.BLL.Models;

namespace HotelsBooking.BLL.Interfaces
{
    public interface IBookingService
    {
        Task<string> CreateBookingAsync(CreateBookingDTO creatingBooking, string userEmail, CancellationToken ct = default);
        Task<BookingDTO> GetBookingByIdAsync(int id, CancellationToken ct = default);
        Task<IEnumerable<BookingDTO>> GetUserBookingsAsync(int userId, CancellationToken ct = default);
        Task<bool> IsRoomAvailableAsync(int roomId, BookingDatesModel bookingDates, CancellationToken ct = default);
        Task ConfirmBookingAsync(string json, string signature, CancellationToken ct = default);
        Task UpdateBookingStatusAsync(int id, UpdateBookingStatusDTO updatingBookingStatus, CancellationToken ct = default);
        Task CancelBookingAsync(int bookingId, CancellationToken ct = default);
    }
}