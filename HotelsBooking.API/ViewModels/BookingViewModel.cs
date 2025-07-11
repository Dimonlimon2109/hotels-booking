using HotelsBooking.BLL.DTO;
using HotelsBooking.DAL.Constants;

namespace HotelsBooking.API.ViewModels
{
    public class BookingViewModel
    {
        public int Id { get; set; }
        public RoomDTO Room { get; set; }
        public UserDTO User { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int Adults { get; set; }
        public int Children { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
    }
}
