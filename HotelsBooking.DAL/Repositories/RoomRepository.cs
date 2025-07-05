
using HotelsBooking.DAL.Data;
using HotelsBooking.DAL.Entities;
using HotelsBooking.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelsBooking.DAL.Repositories
{
    public class RoomRepository: Repository<Room>, IRoomRepository
    {
        public RoomRepository(ApplicationContext context) : base(context) { }

        public async Task<Room?> GetRoomAsync(int hotelId, int roomId, CancellationToken ct = default)
        {
            return await _dbSet
                .Include(r => r.Photos)
                .AsNoTracking()
                .Where(r => r.HotelId == hotelId && r.Id == roomId)
                .FirstOrDefaultAsync(ct);
        }

        public async Task<IEnumerable<Room>> GetRoomsAsync(int hotelId, CancellationToken ct = default)
        {
            return await _dbSet.Include(r => r.Photos).AsNoTracking().Where(r => r.HotelId == hotelId).ToListAsync(ct);
        }
    }
}
