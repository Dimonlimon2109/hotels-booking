using HotelsBooking.BLL.DTO;
using HotelsBooking.BLL.Models;

namespace HotelsBooking.BLL.Interfaces
{
    public interface IHotelService
    {
        Task<HotelDTO> CreateHotelAsync(string userEmail, CreateHotelDTO creatingHotel, CancellationToken ct = default);
        Task DeleteHotelAsync(int id, string userEmail, CancellationToken ct = default);
        Task DeleteHotelPhotoAsync(int hotelId, int photoId, string userEmail, CancellationToken ct = default);
        Task<IEnumerable<HotelDTO>> GetAllHotelsAsync(HotelFiltersModel filters, CancellationToken ct = default);
        Task<HotelDTO> GetHotelAsync(int id, CancellationToken ct = default);
        Task<IEnumerable<HotelDTO>> GetMyHotelsAsync(string userEmail, CancellationToken ct = default);
        Task<int> GetTotalPagesAsync(HotelFiltersModel filters, CancellationToken ct = default);
        Task UpdateHotelAsync(int id, string userEmail, UpdateHotelDTO updatingHotel, CancellationToken ct = default);
        Task UploadHotelPhotoAsync(int id, IImageFile image, string userEmail, CancellationToken ct = default);
    }
}