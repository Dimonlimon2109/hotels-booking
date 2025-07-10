
using HotelsBooking.DAL.Constants;
using HotelsBooking.DAL.Data;
using HotelsBooking.DAL.Entities;
using HotelsBooking.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelsBooking.DAL.Repositories
{
    public class RoomRepository: Repository<Room>, IRoomRepository
    {
        public RoomRepository(ApplicationContext context) : base(context) { }

        public async Task<Room?> GetRoomAsync(int roomId, CancellationToken ct = default)
        {
            return await _dbSet
                .Include(r => r.Photos)
                .AsNoTracking()
                .Where(r => r.Id == roomId)
                .FirstOrDefaultAsync(ct);
        }

        public async Task<IEnumerable<Room>> GetRoomsAsync(int hotelId, CancellationToken ct = default)
        {
            return await _dbSet.Include(r => r.Photos).AsNoTracking().Where(r => r.HotelId == hotelId).ToListAsync(ct);
        }

        public async Task<IEnumerable<Room>> GetAllHotelsWithFiltersAsync(
            int hotelId,
            int limit,
            int offset,
            RoomType? type,
            decimal? minPrice,
            decimal? maxPrice,
            int? capacity,
            string? sortBy,
            string? order,
            CancellationToken ct = default)
        {
            var query = _dbSet.AsQueryable().Where(r => r.HotelId == hotelId);

            if(type != null)
            {
                query = query.Where(r => r.Type == type);
            }

            if(minPrice.HasValue && maxPrice.HasValue)
            {
                query = query.Where(r => r.PricePerNight >= minPrice && r.PricePerNight <= maxPrice);
            }

            if(capacity.HasValue)
            {
                query = query.Where(r => r.Capacity == capacity);
            }

            if (sortBy != null && order != null)
            {
                var orderLower = order.ToLower();
                switch (sortBy.ToLower())
                {
                    case "price":
                        query = orderLower == "desc" ? query.OrderByDescending(h => h.PricePerNight)
                            : query.OrderBy(h => h.PricePerNight);
                        break;

                    default:
                        query = query.OrderBy(h => h.Id);
                        break;
                }
            }

            return await query.Include(h => h.Photos)
                .Skip(offset)
                .Take(limit)
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<int> GetRoomsTotalCountAsync(
            int hotelId,
            RoomType? type,
            decimal? minPrice,
            decimal? maxPrice,
            int? capacity,
            CancellationToken ct = default)
        {
            var query = _dbSet.AsQueryable().Where(r => r.HotelId == hotelId);

            if (type != null)
            {
                query = query.Where(r => r.Type == type);
            }

            if (minPrice.HasValue && maxPrice.HasValue)
            {
                query = query.Where(r => r.PricePerNight >= minPrice && r.PricePerNight <= maxPrice);
            }

            if (capacity.HasValue)
            {
                query = query.Where(r => r.Capacity == capacity);
            }

            return await query.CountAsync(ct);
        }
    }
}
