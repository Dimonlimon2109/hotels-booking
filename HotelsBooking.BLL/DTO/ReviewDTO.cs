
namespace HotelsBooking.BLL.DTO
{
    public class ReviewDTO
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public UserDTO User { get; set; }
    }
}
