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
    public class HotelPhotoToHotelPhotoDtoMapper : Profile
    {
        public HotelPhotoToHotelPhotoDtoMapper()
        {
            CreateMap<HotelPhoto, HotelPhotoDTO>();
        }
    }
}
