using HotelsBooking.BLL.DTO;

namespace HotelsBooking.BLL.Interfaces
{
    public interface IAmenityService
    {
        Task CreateAmenityAsync(CreateAmenityDTO creatingAmenity, CancellationToken ct = default);
        Task DeleteAmenityAsync(int id, CancellationToken ct = default);
        Task<IEnumerable<AmenityDTO>> GetAllAmenitiesAsync(CancellationToken ct = default);
        Task<AmenityDTO> GetAmenityByIdAsync(int id, CancellationToken ct = default);
        Task UpdateAmenityAsync(int id, UpdateAmenityDTO updatingAmenity, CancellationToken ct = default);
    }
}