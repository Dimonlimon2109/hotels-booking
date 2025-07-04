
using AutoMapper;
using HotelsBooking.BLL.DTO;
using HotelsBooking.DAL.Entities;

namespace HotelsBooking.BLL.Mappers
{
    public class RoomToRoomDtoMapper : Profile
    {
        public RoomToRoomDtoMapper()
        {
            CreateMap<Room, RoomDTO>();
        }
    }
}
