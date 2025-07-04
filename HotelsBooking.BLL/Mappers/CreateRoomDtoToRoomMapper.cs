
using AutoMapper;
using HotelsBooking.BLL.DTO;
using HotelsBooking.DAL.Entities;

namespace HotelsBooking.BLL.Mappers
{
    public class CreateRoomDtoToRoomMapper : Profile
    {
        public CreateRoomDtoToRoomMapper()
        {
            CreateMap<CreateRoomDTO, Room>();
        }
    }
}
