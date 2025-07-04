using AutoMapper;
using HotelsBooking.BLL.DTO;
using HotelsBooking.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelsBooking.BLL.Mappers
{
    public class HotelPhotoToPhotoDtoMapper : Profile
    {
        public HotelPhotoToPhotoDtoMapper()
        {
            CreateMap<HotelPhoto, PhotoDTO>();
        }
    }
}
