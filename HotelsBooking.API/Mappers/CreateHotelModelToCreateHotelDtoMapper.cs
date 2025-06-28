using AutoMapper;
using HotelsBooking.API.Models;
using HotelsBooking.BLL.DTO;

namespace HotelsBooking.API.Mappers
{
    public class CreateHotelModelToCreateHotelDtoMapper: Profile
    {
        public CreateHotelModelToCreateHotelDtoMapper()
        {
            CreateMap<CreateHotelModel, CreateHotelDTO>();
        }
    }
}
