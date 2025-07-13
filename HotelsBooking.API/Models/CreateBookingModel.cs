namespace HotelsBooking.API.Models
{
    public record CreateBookingModel
        (
        int RoomId,
        DateTime CheckInDate,
        DateTime CheckOutDate,
        int Adults,
        int Children
        );
}
