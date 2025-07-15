
using HotelsBooking.DAL.Entities;

namespace HotelsBooking.DAL.Interfaces
{
    public interface IReviewRepository : IRepository<Review>
    {
        Task<IEnumerable<Review>> GetReviewsWithUserAsync(int hotelId, CancellationToken ct = default);
        Task<int> CountHotelReviewsAsync(int hotelId, CancellationToken ct = default);
    }
}
