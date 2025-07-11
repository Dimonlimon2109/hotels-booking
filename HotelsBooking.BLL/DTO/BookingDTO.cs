
using HotelsBooking.DAL.Constants;

namespace HotelsBooking.BLL.DTO
{
    public class BookingDTO
    {
        public int Id { get; set; }
        public RoomDTO Room { get; set; }
        public UserDTO User { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int Adults { get; set; }
        public int Children { get; set; }
        public decimal TotalPrice { get; set; }
        public BookingStatus Status { get; set; }
    }
}
