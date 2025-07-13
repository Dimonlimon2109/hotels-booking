using HotelsBooking.BLL.DTO;

namespace HotelsBooking.BLL.Interfaces
{
    public interface IReviewService
    {
        Task CreateReviewAsync(CreateReviewDTO creatingReview, string userEmail, CancellationToken ct = default);
        Task DeleteReviewAsync(int id, string userEmail, CancellationToken ct = default);
        Task<IEnumerable<ReviewDTO>> GetAllReviewsByHotelAsync(int hotelId, CancellationToken ct = default);
        Task UpdateReviewAsync(int id, UpdateReviewDTO updatingReview, string userEmail, CancellationToken ct = default);
    }
}