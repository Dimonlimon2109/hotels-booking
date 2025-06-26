using AutoMapper;
using HotelsBooking.BLL.DTO;
using HotelsBooking.BLL.Services;
using HotelsBooking.DAL.Entities;

namespace HotelsBooking.BLL.Mappers
{
    public class RegisterDtoToUserMapper : Profile
    {
        public RegisterDtoToUserMapper()
        {
            CreateMap<RegisterDTO, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => new PasswordService().HashPassword(src.Password)));
        }
    }
}
