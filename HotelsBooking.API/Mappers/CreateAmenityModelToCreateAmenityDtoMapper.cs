using AutoMapper;
using HotelsBooking.API.Models;
using HotelsBooking.BLL.DTO;

namespace HotelsBooking.API.Mappers
{
    public class CreateAmenityModelToCreateAmenityDtoMapper: Profile
    {
        public CreateAmenityModelToCreateAmenityDtoMapper()
        {
            CreateMap<CreateAmenityModel, CreateAmenityDTO>();
        }
    }
}
