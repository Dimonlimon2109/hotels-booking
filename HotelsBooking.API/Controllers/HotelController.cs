using AutoMapper;
using HotelsBooking.API.Models;
using HotelsBooking.API.ViewModels;
using HotelsBooking.BLL.DTO;
using HotelsBooking.BLL.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

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
            var hotelDTO = await _hotelService.CreateHotelAsync(createHotelDTO, ct);
            var hotelViewModel = _mapper.Map<HotelViewModel>(hotelDTO);
            return Created();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllHotels(CancellationToken ct = default)
        {
            var hotelsDTO = await _hotelService.GetAllHotelsAsync(ct);
            var hotelsViewModel = hotelsDTO.Select(h => _mapper.Map<HotelViewModel>(h));
            return Ok(hotelsViewModel);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetSingleHotel(int id, CancellationToken ct)
        {
            var hotelDTO = await _hotelService.GetHotelAsync(id, ct);

            return Ok(_mapper.Map<HotelViewModel>(hotelDTO));
        }
    }
}
