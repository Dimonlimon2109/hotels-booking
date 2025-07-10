
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

        public async Task<Hotel?> GetHotelAsync(int id, CancellationToken ct = default)
        {
            return await _dbSet.Include(h => h.Photos).Include(h => h.Amenities).AsNoTracking().FirstOrDefaultAsync(h => h.Id == id);
        }

        public async Task<IEnumerable<Hotel>> GetOwnerHotelsAsync(int id, CancellationToken ct = default)
        {
            return await _dbSet.Include(h => h.Owner).Where(h=> h.OwnerId == id).AsNoTracking().ToListAsync(ct);
        }

        public async Task<IEnumerable<Hotel>> GetHotelsAsync(CancellationToken ct = default)
        {
            return await _dbSet.Include(h => h.Photos).Include(h => h.Amenities).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Hotel>> GetAllHotelsWithFiltersAsync(
            int limit,
            int offset,
            string? name,
            string? city,
            int? starRating,
            IEnumerable<int>? amenityIds,
            string? sortBy,
            string? order,
            CancellationToken ct = default
            )
        {
            var query = _dbSet.AsQueryable();

            if (name != null)
            {
                query = query.Where(h => h.Name.Contains(name));
            }

            if (city != null)
            {
                query = query.Where(h => h.City == city);
            }

            if (starRating.HasValue)
            {
                query = query.Where(h => h.StarRating == starRating);
            }

            if (amenityIds != null && amenityIds.Any())
            {
                query = query.Where(h =>
                    amenityIds.All(id => h.Amenities.Select(a => a.Id).Contains(id))
                );
            }

            if (sortBy != null && order != null)
            {
                var orderLower = order.ToLower();
                switch (sortBy.ToLower())
                {
                    case "rating":
                        query = orderLower == "desc" ? query.OrderByDescending(h => h.ReviewRating)
                            : query.OrderBy(h => h.ReviewRating);
                        break;

                    //case "distance":
                    //    if(centerLatitude.HasValue && centerLongitude.HasValue)
                    //    {
                    //        double latCenter = centerLatitude.Value;
                    //        double lonCenter = centerLongitude.Value;

                    //        query = orderLower == "desc" ? query.OrderByDescending(h => GetDistanceKm(latCenter, lonCenter, (double)h.Latitude, (double)h.Longitude))
                    //            : query.OrderBy(h => GetDistanceKm(latCenter, lonCenter, (double)h.Latitude, (double)h.Longitude));
                    //    }
                    //    break;

                    default:
                        query = query.OrderBy(h => h.Id);
                        break;
                }
            }
            return await query.Include(h => h.Amenities)
                .Include(h => h.Photos)
                .Include(h => h.Reviews)
                .Skip(offset)
                .Take(limit)
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<int> GetHotelsTotalCountAsync(
            string? name,
            string? city,
            int? starRating,
            IEnumerable<int>? amenityIds,
            CancellationToken ct = default)
        {
            var query = _dbSet.AsQueryable();

            if (name != null)
            {
                query = query.Where(h => h.Name.Contains(name));
            }

            if (city != null)
            {
                query = query.Where(h => h.City == city);
            }

            if (starRating.HasValue)
            {
                query = query.Where(h => h.StarRating == starRating);
            }

            if (amenityIds != null && amenityIds.Any())
            {
                query = query.Where(h =>
                    amenityIds.All(id => h.Amenities.Select(a => a.Id).Contains(id))
                );
            }
            return await query.CountAsync(ct);
        }



        //private static double GetDistanceKm(double lat1, double lon1, double lat2, double lon2)
        //{
        //    const double R = 6371;
        //    double dLat = DegreesToRadians(lat2 - lat1);
        //    double dLon = DegreesToRadians(lon2 - lon1);

        //    double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
        //               Math.Cos(DegreesToRadians(lat1)) * Math.Cos(DegreesToRadians(lat2)) *
        //               Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        //    double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        //    return R * c;
        //}

        //private static double DegreesToRadians(double deg) => deg * Math.PI / 180;
    }
}
