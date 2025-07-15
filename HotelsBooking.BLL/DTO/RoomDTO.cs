
namespace HotelsBooking.BLL.DTO
{
    public class RoomDTO
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public decimal PricePerNight { get; set; }
        public int Capacity { get; set; }
        public IEnumerable<PhotoDTO> Photos { get; set; }
    }
}
