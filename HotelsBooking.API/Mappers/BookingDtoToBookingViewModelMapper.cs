using AutoMapper;
using HotelsBooking.API.ViewModels;
using HotelsBooking.BLL.DTO;

namespace HotelsBooking.API.Mappers
{
    public class BookingDtoToBookingViewModelMapper : Profile
    {
        public BookingDtoToBookingViewModelMapper()
        {
            CreateMap<BookingDTO, BookingViewModel>();
        }
    }
}
