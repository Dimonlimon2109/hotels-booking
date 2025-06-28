using AutoMapper;
using HotelsBooking.API.Models;
using HotelsBooking.API.ViewModels;
using HotelsBooking.BLL.DTO;
using HotelsBooking.BLL.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace HotelsBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController: ControllerBase
    {
        private readonly HotelService _hotelService;
        private readonly IMapper _mapper;

        public HotelController(
            HotelService hotelService,
            IMapper mapper)
        {
            _hotelService = hotelService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateHotel(CreateHotelModel creatingHotel, CancellationToken ct = default)
        {
            var createHotelDTO = _mapper.Map<CreateHotelDTO>(creatingHotel);
            var hotelDTO = await _hotelService.CreateHotelAsync(createHotelDTO);
            var hotelViewModel = _mapper.Map<HotelViewModel>(hotelDTO);
            return Created();
        }
    }
}
