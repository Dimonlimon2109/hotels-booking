using AutoMapper;
using HotelsBooking.API.Models;
using HotelsBooking.BLL.DTO;

namespace HotelsBooking.API.Mappers
{
    public class LoginModelToLoginDtoMapper : Profile
    {
        public LoginModelToLoginDtoMapper()
        {
            CreateMap<LoginModel, LoginDTO>();
        }
    }
}
