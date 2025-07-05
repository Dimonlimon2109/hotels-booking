
namespace HotelsBooking.BLL.DTO
{
    public class CreateReviewDTO
    {
        public int HotelId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
    }
}
