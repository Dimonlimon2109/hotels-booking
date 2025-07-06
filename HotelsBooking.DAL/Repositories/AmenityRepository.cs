
using HotelsBooking.DAL.Data;
using HotelsBooking.DAL.Entities;
using HotelsBooking.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HotelsBooking.DAL.Repositories
{
    public class AmenityRepository : Repository<Amenity>, IAmenityRepository
    {
        public AmenityRepository(ApplicationContext applicationContext) : base(applicationContext) { }

        public async Task<IEnumerable<Amenity>> GetExistingAmenitiesAsync(IEnumerable<int> ids, CancellationToken ct = default)
        {
            return await _dbSet.Where(a => ids.Contains(a.Id)).ToListAsync(ct);
        }
    }
}
