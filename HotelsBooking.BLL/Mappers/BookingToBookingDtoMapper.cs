
using AutoMapper;
using HotelsBooking.BLL.DTO;
using HotelsBooking.DAL.Entities;

namespace HotelsBooking.BLL.Mappers
{
    public class BookingToBookingDtoMapper : Profile
    {
        public BookingToBookingDtoMapper()
        {
            CreateMap<Booking, BookingDTO>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
        }
    }
}
