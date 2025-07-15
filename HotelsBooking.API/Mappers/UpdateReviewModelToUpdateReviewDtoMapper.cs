using AutoMapper;
using HotelsBooking.API.Models;
using HotelsBooking.BLL.DTO;

namespace HotelsBooking.API.Mappers
{
    public class UpdateReviewModelToUpdateReviewDtoMapper : Profile
    {
        public UpdateReviewModelToUpdateReviewDtoMapper()
        {
            CreateMap<UpdateReviewModel, UpdateReviewDTO>();
        }
    }
}
