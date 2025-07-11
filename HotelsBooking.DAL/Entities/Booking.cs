
using HotelsBooking.DAL.Constants;

namespace HotelsBooking.DAL.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public string UserId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int Adults { get; set; }
        public int Children { get; set; }
        public decimal TotalPrice { get; set; }
        public BookingStatus Status { get; set; }
        public User User { get; set; }
        public Room Room { get; set; }
    }
}
