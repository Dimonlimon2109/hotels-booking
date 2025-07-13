
using HotelsBooking.DAL.Constants;
using HotelsBooking.DAL.Data;
using HotelsBooking.DAL.Entities;
using HotelsBooking.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelsBooking.DAL.Repositories
{
    public class BookingRepository(ApplicationContext context) : Repository<Booking>(context), IBookingRepository
    {
        public async Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(int userId, CancellationToken ct = default)
        {
            return await _dbSet.Include(b => b.User).Include(b => b.Room).Where(b => b.UserId == userId).AsNoTracking().ToListAsync();
        }

        new public async Task<Booking?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _dbSet.Include(b => b.User).Include(b => b.Room).FirstOrDefaultAsync(b => b.Id == id, ct);
        }

        public async Task<int> GetOverlappingBookingsCountAsync(
            int roomId,
            DateTime checkIn,
            DateTime checkOut,
            CancellationToken ct = default)
        {
            return await _dbSet
                .Where(b =>
                    b.RoomId == roomId &&
                    b.Status != BookingStatus.Cancelled &&
                    b.CheckInDate < checkOut &&
                    b.CheckOutDate > checkIn
                ).CountAsync(ct);
        }

        public async Task<bool> IsRoomAvailableAsync(
            int roomId,
            DateTime checkInDate,
            DateTime checkOutDate,
            CancellationToken ct = default
            )
        {
            return !await _dbSet.AnyAsync(b =>
            b.RoomId == roomId &&
            b.Status != BookingStatus.Cancelled &&
            (
                (checkInDate < b.CheckOutDate && checkOutDate > b.CheckInDate)
            ), ct);
        }

        public async Task<IEnumerable<Booking>> GetByRoomIdAsync(int roomId, CancellationToken ct = default)
        {
            return await _dbSet.Where(b => b.RoomId == roomId).AsNoTracking().ToListAsync();
        }

    }
}
