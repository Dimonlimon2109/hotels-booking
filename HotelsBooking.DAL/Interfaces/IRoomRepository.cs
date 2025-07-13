
using HotelsBooking.DAL.Constants;
using HotelsBooking.DAL.Entities;

namespace HotelsBooking.DAL.Interfaces
{
    public interface IRoomRepository: IRepository<Room>
    {
        Task<IEnumerable<Room>> GetRoomsAsync(int hotelId, CancellationToken ct = default);
        Task<Room?> GetRoomAsync(int roomId, CancellationToken ct = default);

        Task<IEnumerable<Room>> GetAllHotelsWithFiltersAsync(
            int hotelId,
            int limit,
            int offset,
            RoomType? type,
            decimal? minPrice,
            decimal? maxPrice,
            int? capacity,
            string? sortBy,
            string? order,
            CancellationToken ct = default);
        Task<int> GetRoomsTotalCountAsync(
            int hotelId,
            RoomType? type,
            decimal? minPrice,
            decimal? maxPrice,
            int? capacity,
            CancellationToken ct = default);
    }
}
