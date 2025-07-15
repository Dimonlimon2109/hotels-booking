using AutoMapper;
using HotelsBooking.API.Models;
using HotelsBooking.BLL.DTO;

namespace HotelsBooking.API.Mappers
{
    public class UpdateBookingStatusModelToUpdateBookingStatusDtoMapper : Profile
    {
        public UpdateBookingStatusModelToUpdateBookingStatusDtoMapper()
        {
            CreateMap<UpdateBookingStatusModel, UpdateBookingStatusDTO>();
        }
    }
}
