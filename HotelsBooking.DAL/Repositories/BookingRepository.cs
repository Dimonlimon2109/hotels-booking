
using HotelsBooking.DAL.Data;
using HotelsBooking.DAL.Entities;
using HotelsBooking.DAL.Interfaces;

namespace HotelsBooking.DAL.Repositories
{
    public class BookingRepository(ApplicationContext context) : Repository<Booking>(context), IBookingRepository
    {
    }
}
