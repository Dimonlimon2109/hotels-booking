using HotelsBooking.DAL.Data;
using HotelsBooking.DAL.Entities;
using HotelsBooking.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelsBooking.DAL.Repositories
{
    public class ReviewRepository : Repository<Review>, IReviewRepository
    {
        public ReviewRepository(ApplicationContext context): base(context) { }

        public async Task<IEnumerable<Review>> GetReviewsWithUserAsync(int hotelId, CancellationToken ct = default)
        {
            return await _dbSet.Include(r => r.User).AsNoTracking().Where(r => r.HotelId == hotelId).ToListAsync(ct);
        }

        public async Task<int> CountHotelReviewsAsync(int hotelId, CancellationToken ct = default)
        {
            return await _dbSet.Where(r => r.HotelId == hotelId).CountAsync(ct);
        }
    }
}
