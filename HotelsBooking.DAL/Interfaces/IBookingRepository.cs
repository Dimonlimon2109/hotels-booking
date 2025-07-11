
using HotelsBooking.DAL.Entities;

namespace HotelsBooking.DAL.Interfaces
{
    public interface IBookingRepository : IRepository<Booking>
    {
        Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(int userId, CancellationToken ct = default);
        Task<int> GetOverlappingBookingsCountAsync(
            int roomId,
            DateTime checkIn,
            DateTime checkOut,
            CancellationToken ct = default);

        Task<bool> IsRoomAvailableAsync(
            int roomId,
            DateTime checkInDate,
            DateTime checkOutDate,
            CancellationToken ct = default
            );
        }
}
