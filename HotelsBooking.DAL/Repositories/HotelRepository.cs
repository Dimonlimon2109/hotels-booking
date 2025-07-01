
using HotelsBooking.DAL.Data;
using HotelsBooking.DAL.Entities;
using HotelsBooking.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelsBooking.DAL.Repositories
{
    public class HotelRepository : Repository<Hotel>, IHotelRepository
    {
        public HotelRepository(ApplicationContext context) : base(context) { }

        public async Task<Hotel?> GetHotelByIdWithOwnerAsync(int id, CancellationToken ct = default)
        {
            return await _dbSet.Include(h => h.Owner).AsNoTracking().FirstOrDefaultAsync(h => h.Id == id);
        }

        public async Task<Hotel?> GetSingleHotelWithPhotosAsync(int id, CancellationToken ct = default)
        {
            return await _dbSet.Include(h => h.Photos).AsNoTracking().FirstOrDefaultAsync(h => h.Id == id);
        }

        public async Task<IEnumerable<Hotel>> GetOwnerHotelsAsync(int id, CancellationToken ct = default)
        {
            return await _dbSet.Include(h => h.Owner).Where(h=> h.OwnerId == id).AsNoTracking().ToListAsync(ct);
        }

        public async Task<IEnumerable<Hotel>> GetHotelsWithPhotosAsync(CancellationToken ct = default)
        {
            return await _dbSet.Include(h => h.Photos).AsNoTracking().ToListAsync();
        }
    }
}
