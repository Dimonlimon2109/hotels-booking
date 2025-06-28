using AutoMapper;
using HotelsBooking.API.Models;
using HotelsBooking.BLL.DTO;

namespace HotelsBooking.API.Mappers
{
    public class UpdateHotelModelToUpdateHotelDtoMapper : Profile
    {
        public UpdateHotelModelToUpdateHotelDtoMapper()
        {
            CreateMap<UpdateHotelModel, UpdateHotelDTO>();
        }
    }
}
