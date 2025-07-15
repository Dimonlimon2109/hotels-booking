using AutoMapper;
using HotelsBooking.API.Models;
using HotelsBooking.BLL.DTO;

namespace HotelsBooking.API.Mappers
{
    public class UpdateAmenityModelToUpdateAmenityDtoMapper: Profile
    {
        public UpdateAmenityModelToUpdateAmenityDtoMapper()
        {
            CreateMap<UpdateAmenityModel, UpdateAmenityDTO>();
        }
    }
}
