using HotelsBooking.DAL.Entities;

namespace HotelsBooking.API.ViewModels
{
    public class RoomViewModel
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public decimal PricePerNight { get; set; }
        public int Capacity { get; set; }
        public IEnumerable<PhotoViewModel> Photos { get; set; }

    }
}
