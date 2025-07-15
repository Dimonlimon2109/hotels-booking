namespace HotelsBooking.API.ViewModels
{
    public class ReviewViewModel
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public UserViewModel User { get; set; }
    }
}
