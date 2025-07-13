using AutoMapper;
using HotelsBooking.API.Models;
using HotelsBooking.BLL.DTO;

namespace HotelsBooking.API.Mappers
{
    public class CreateBookingModelToCreateBookingDtoMapper : Profile
    {
        public CreateBookingModelToCreateBookingDtoMapper()
        {
            CreateMap<CreateBookingModel, CreateBookingDTO>();
        }
    }
}
