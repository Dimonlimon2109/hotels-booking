namespace HotelsBooking.API.Models
{
    public record CreateReviewModel
        (
        int HotelId,
        int rating,
        string Comment
        );
}
