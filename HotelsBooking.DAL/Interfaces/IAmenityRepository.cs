
using HotelsBooking.DAL.Entities;

namespace HotelsBooking.DAL.Interfaces
{
    public interface IAmenityRepository : IRepository<Amenity>
    {
        Task<IEnumerable<Amenity>> GetExistingAmenitiesAsync(IEnumerable<int> ids, CancellationToken ct = default);

    }
}
