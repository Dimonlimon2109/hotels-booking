using AutoMapper;
using HotelsBooking.API.Models;
using HotelsBooking.BLL.DTO;

namespace HotelsBooking.API.Mappers
{
    public class RegisterModelToRegisterDtoMapper : Profile
    {
        public RegisterModelToRegisterDtoMapper()
        {
            CreateMap<RegisterModel, RegisterDTO>();
        }
    }
}
