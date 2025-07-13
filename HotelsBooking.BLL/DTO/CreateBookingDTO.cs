
namespace HotelsBooking.BLL.DTO
{
    public class CreateBookingDTO
    {
        public int RoomId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int Adults { get; set; }
        public int Children { get; set; }
    }
}
