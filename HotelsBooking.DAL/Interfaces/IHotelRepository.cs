
using HotelsBooking.DAL.Entities;

namespace HotelsBooking.DAL.Interfaces
{
    public interface IHotelRepository : IRepository<Hotel>
    {
        Task<Hotel?> GetHotelByIdWithOwnerAsync(int id, CancellationToken ct = default);
        Task<Hotel?> GetHotelAsync(int id, CancellationToken ct = default);
        Task<IEnumerable<Hotel>> GetOwnerHotelsAsync(int id, CancellationToken ct = default);
        Task<IEnumerable<Hotel>> GetHotelsAsync(CancellationToken ct = default);
    }
}