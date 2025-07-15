using HotelsBooking.DAL.Entities;

namespace HotelsBooking.BLL.Interfaces
{
    public interface IPdfGenerator
    {
        byte[] GenerateBookingConfirmation(Booking booking);
    }
}