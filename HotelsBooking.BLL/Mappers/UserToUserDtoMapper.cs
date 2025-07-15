
using AutoMapper;
using HotelsBooking.BLL.DTO;
using HotelsBooking.DAL.Entities;

namespace HotelsBooking.BLL.Mappers
{
    public class UserToUserDtoMapper : Profile
    {
        public UserToUserDtoMapper()
        {
            CreateMap<User, UserDTO>();
        }
    }
}
