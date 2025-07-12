using HotelsBooking.BLL.DTO;
using HotelsBooking.BLL.Models;

namespace HotelsBooking.BLL.Interfaces
{
    public interface IRoomService
    {
        Task<RoomDTO> CreateRoomAsync(string userEmail, CreateRoomDTO creatingRoom, CancellationToken ct = default);
        Task DeleteRoomAsync(int id, string userEmail, CancellationToken ct = default);
        Task DeleteRoomPhotoAsync(int roomId, int photoId, string userEmail, CancellationToken ct = default);
        Task<RoomDTO> GetRoomByIdAsync(int roomId, CancellationToken ct = default);
        Task<IEnumerable<RoomDTO>> GetRoomsByHotelIdAsync(int hotelId, RoomFiltersModel filters, CancellationToken ct = default);
        Task<int> GetTotalPagesAsync(int hotelId, RoomFiltersModel filters, CancellationToken ct = default);
        Task UpdateRoomAsync(int roomId, string userEmail, UpdateRoomDTO updatingRoom, CancellationToken ct = default);
        Task UploadRoomPhotoAsync(int id, IImageFile image, string userEmail, CancellationToken ct = default);
    }
}