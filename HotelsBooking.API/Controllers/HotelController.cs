using AutoMapper;
using HotelsBooking.API.Constants;
using HotelsBooking.API.Models;
using HotelsBooking.API.ViewModels;
using HotelsBooking.BLL.DTO;
using HotelsBooking.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

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

        [Authorize(Policy = Policies.HotelOwner)]
        [HttpPost]
        public async Task<IActionResult> CreateHotel(CreateHotelModel creatingHotel, CancellationToken ct = default)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var createHotelDTO = _mapper.Map<CreateHotelDTO>(creatingHotel);
            var hotelDTO = await _hotelService.CreateHotelAsync(userEmail, createHotelDTO, ct);
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

        [Authorize(Policy = Policies.HotelOwner)]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteHotel(int id, CancellationToken ct)
        {
            await _hotelService.DeleteHotelAsync(id, ct);
            return NoContent();
        }

        [Authorize(Policy = Policies.HotelOwner)]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateHotel(int id, UpdateHotelModel updatingHotel, CancellationToken ct)
        {
            var updatingHotelDTO = _mapper.Map<UpdateHotelDTO>(updatingHotel);
            await _hotelService.UpdateHotelAsync(id, updatingHotelDTO, ct);

            return Ok();
        }
    }
}
