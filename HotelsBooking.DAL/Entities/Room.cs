
using HotelsBooking.DAL.Constants;

namespace HotelsBooking.DAL.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public int HotelId { get; set; }
        public RoomType Type { get; set; }
        public decimal PricePerNight { get; set; }
        public int Capacity { get; set; }
        public Hotel Hotel { get; set; }
        public ICollection<RoomPhoto> Photos { get; set; }
    }
}
