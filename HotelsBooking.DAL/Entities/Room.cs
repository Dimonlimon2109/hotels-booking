
namespace HotelsBooking.DAL.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public int HotelId { get; set; }
        public string Type { get; set; } // "Standard", "Deluxe" и т.д.
        public decimal PricePerNight { get; set; }
        public int Capacity { get; set; }
        public Hotel Hotel { get; set; }
        public ICollection<RoomPhoto> Photos { get; set; }
    }
}
