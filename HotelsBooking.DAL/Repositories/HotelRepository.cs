
using HotelsBooking.DAL.Data;
using HotelsBooking.DAL.Entities;
using HotelsBooking.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using NetTopologySuite;

namespace HotelsBooking.DAL.Repositories
{
    public class HotelRepository(ApplicationContext context) : Repository<Hotel>(context), IHotelRepository
    {
        public async Task<Hotel?> GetHotelByIdWithOwnerAsync(int id, CancellationToken ct = default)
        {
            return await _dbSet.Include(h => h.Owner).AsNoTracking().FirstOrDefaultAsync(h => h.Id == id, ct);
        }

        public async Task<Hotel?> GetHotelAsync(int id, CancellationToken ct = default)
        {
            return await _dbSet.Include(h => h.Photos).Include(h => h.Amenities).AsNoTracking().FirstOrDefaultAsync(h => h.Id == id, ct);
        }

        public async Task<IEnumerable<Hotel>> GetOwnerHotelsAsync(int id, CancellationToken ct = default)
        {
            return await _dbSet.Include(h => h.Owner).Where(h=> h.OwnerId == id).AsNoTracking().ToListAsync(ct);
        }

        public async Task<IEnumerable<Hotel>> GetHotelsAsync(CancellationToken ct = default)
        {
            return await _dbSet.Include(h => h.Photos).Include(h => h.Amenities).AsNoTracking().ToListAsync(ct);
        }

        public async Task<IEnumerable<Hotel>> GetAllHotelsWithFiltersAsync(
            int limit,
            int offset,
            string? name,
            string? city,
            int? starRating,
            IEnumerable<int>? amenityIds,
            double? centerLatitude,
            double? centerLongitude,
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

                    case "distance":
                        if (centerLatitude.HasValue && centerLongitude.HasValue)
                        {
                            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
                            var centerPoint = geometryFactory.CreatePoint(new Coordinate(centerLongitude.Value, centerLatitude.Value));

                            query = orderLower == "desc"
                                ? query.OrderByDescending(h => h.Location.Distance(centerPoint))
                                : query.OrderBy(h => h.Location.Distance(centerPoint));
                        }
                        break;

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

        public void Update(
            Hotel hotel,
            double latitude,
            double longitude)
        {
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
            var point = geometryFactory.CreatePoint(new Coordinate(longitude, latitude));
            hotel.Location = point;
            _dbSet.Entry(hotel).State = EntityState.Modified;
        }

        public async Task AddAsync(
            Hotel hotel, 
            double latitude,
            double longitude,
            CancellationToken ct = default)
        {
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
            var point = geometryFactory.CreatePoint(new Coordinate(longitude, latitude));
            hotel.Location = point;
            await _dbSet.AddAsync(hotel, ct);
        }
    }
}
