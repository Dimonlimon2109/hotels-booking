
using AutoMapper;
using HotelsBooking.BLL.DTO;
using HotelsBooking.DAL.Entities;

namespace HotelsBooking.BLL.Mappers
{
    public class RoomPhotoToPhotoDtoMapper: Profile
    {
        public RoomPhotoToPhotoDtoMapper()
        {
            CreateMap<RoomPhoto, PhotoDTO>();
        }
    }
}
