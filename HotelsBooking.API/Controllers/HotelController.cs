using AutoMapper;
using HotelsBooking.API.Adapters;
using HotelsBooking.API.Constants;
using HotelsBooking.API.Models;
using HotelsBooking.API.ViewModels;
using HotelsBooking.BLL.DTO;
using HotelsBooking.BLL.Interfaces;
using HotelsBooking.BLL.Models;
using HotelsBooking.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotelsBooking.API.Controllers
{
    [Route("api/hotels")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly IHotelService _hotelService;
        private readonly IRoomService _roomService;
        private readonly IMapper _mapper;

        public HotelController(
            IHotelService hotelService,
            IRoomService roomService,
            IMapper mapper)
        {
            _hotelService = hotelService;
            _roomService = roomService;
            _mapper = mapper;
        }

        [Authorize(Roles = Roles.HotelOwner)]
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
        public async Task<IActionResult> GetAllHotels([FromQuery] HotelFiltersModel filters, CancellationToken ct = default)
        {
            var hotelsDTO = await _hotelService.GetAllHotelsAsync(filters, ct);
            var totalCount = await _hotelService.GetTotalPagesAsync(filters, ct);
            var hotelsViewModel = hotelsDTO.Select(h => _mapper.Map<HotelViewModel>(h));
            return Ok(new
            {
                hotels = hotelsViewModel,
                totalCount
            });
        }

        [HttpGet("{hotelId:int}/rooms")]
        public async Task<IActionResult> GetAllRoomsByHotelId(
            int hotelId,
            [FromQuery] RoomFiltersModel filters,
            CancellationToken ct = default)
        {
            var roomsDTO = await _roomService.GetRoomsByHotelIdAsync(hotelId, filters, ct);
            var totalCount = await _roomService.GetTotalPagesAsync(hotelId, filters, ct);
            var roomsViewModel = roomsDTO.Select(hd => _mapper.Map<RoomViewModel>(hd));
            return Ok(new
            {
                rooms = roomsViewModel,
                totalCount,
            });
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var hotelDTO = await _hotelService.GetHotelAsync(id, ct);

            return Ok(_mapper.Map<HotelViewModel>(hotelDTO));
        }

        [Authorize(Roles = Roles.HotelOwner)]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteHotel(int id, CancellationToken ct)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            await _hotelService.DeleteHotelAsync(id, userEmail, ct);
            return NoContent();
        }

        [Authorize(Roles = Roles.HotelOwner)]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateHotel(int id, UpdateHotelModel updatingHotel, CancellationToken ct)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var updatingHotelDTO = _mapper.Map<UpdateHotelDTO>(updatingHotel);
            await _hotelService.UpdateHotelAsync(id, userEmail, updatingHotelDTO, ct);

            return Ok();
        }

        [Authorize(Roles = Roles.HotelOwner)]
        [HttpGet("my")]
        public async Task<IActionResult> GetMyHotels(CancellationToken ct)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var hotelsDTO = await _hotelService.GetMyHotelsAsync(userEmail, ct);
            return Ok(hotelsDTO.Select(h => _mapper.Map<HotelViewModel>(h)));
        }

        [Authorize(Roles = Roles.HotelOwner)]
        [HttpPost("{id:int}/photo")]
        public async Task<IActionResult> UploadHotelPhoto(int id, IFormFile photo, CancellationToken ct = default)
        {
            var adapter = new FormFileAdapter(photo);
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            await _hotelService.UploadHotelPhotoAsync(id, adapter, userEmail, ct);
            return Ok();
        }

        [Authorize(Roles = Roles.HotelOwner)]
        [HttpDelete("{hotelId:int}/photo/{photoId:int}")]
        public async Task<IActionResult> DeleteHotelPhoto(int hotelId, int photoId, CancellationToken ct = default)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            await _hotelService.DeleteHotelPhotoAsync(hotelId, photoId, userEmail, ct);
            return NoContent();
        }
    }
}
