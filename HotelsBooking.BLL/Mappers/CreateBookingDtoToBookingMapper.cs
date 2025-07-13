
using AutoMapper;
using HotelsBooking.BLL.DTO;
using HotelsBooking.DAL.Entities;

namespace HotelsBooking.BLL.Mappers
{
    public class CreateBookingDtoToBookingMapper : Profile
    {
        public CreateBookingDtoToBookingMapper()
        {
            CreateMap<CreateBookingDTO, Booking>();
        }
    }
}
