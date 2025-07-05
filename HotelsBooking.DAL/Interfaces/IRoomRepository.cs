
using HotelsBooking.DAL.Entities;

namespace HotelsBooking.DAL.Interfaces
{
    public interface IRoomRepository: IRepository<Room>
    {
        Task<IEnumerable<Room>> GetRoomsAsync(int hotelId, CancellationToken ct = default);
        Task<Room?> GetRoomAsync(int roomId, CancellationToken ct = default);
    }
}
