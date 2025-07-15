using AutoMapper;
using HotelsBooking.API.Models;
using HotelsBooking.BLL.DTO;

namespace HotelsBooking.API.Mappers
{
    public class CreateRoomModelToCreateRoomDtoMapper: Profile
    {
        public CreateRoomModelToCreateRoomDtoMapper()
        {
            CreateMap<CreateRoomModel, CreateRoomDTO>();
        }
    }
}
