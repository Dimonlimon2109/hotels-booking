
using HotelsBooking.DAL.Entities;

namespace HotelsBooking.DAL.Interfaces
{
    public interface IHotelRepository : IRepository<Hotel>
    {
        Task<Hotel?> GetHotelByIdWithOwnerAsync(int id, CancellationToken ct = default);
        Task<Hotel?> GetHotelAsync(int id, CancellationToken ct = default);
        Task<IEnumerable<Hotel>> GetOwnerHotelsAsync(int id, CancellationToken ct = default);
        Task<IEnumerable<Hotel>> GetHotelsAsync(CancellationToken ct = default);
        Task<IEnumerable<Hotel>> GetAllHotelsWithFiltersAsync(
            int limit,
            int offset,
            string? name,
            string? county,
            int? starRating,
            IEnumerable<int> amenityIds,
            string? sortBy,
            string? order,
            CancellationToken ct = default
            );
        Task<int> GetHotelsTotalCountAsync(
            string? name,
            string? city,
            int? starRating,
            IEnumerable<int>? amenityIds,
            CancellationToken ct = default);
    }
}