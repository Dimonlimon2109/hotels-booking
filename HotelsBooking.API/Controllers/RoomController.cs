using AutoMapper;
using HotelsBooking.API.Models;
using HotelsBooking.API.ViewModels;
using HotelsBooking.BLL.DTO;
using HotelsBooking.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotelsBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly RoomService _roomService;
        private readonly IMapper _mapper;

        public RoomController(RoomService roomService, IMapper mapper)
        {
            _roomService = roomService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRoomModel creatingRoom, CancellationToken ct = default)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var createRoomDTO = _mapper.Map<CreateRoomDTO>(creatingRoom);
            var RoomDTO = await _roomService.CreateRoomAsync(userEmail, createRoomDTO, ct);
            return Created();
        }

        [HttpGet("{hotelId:int}")]
        public async Task<IActionResult> GetAll(int hotelId, CancellationToken ct = default)
        {
            var roomsDTO = await _roomService.GetRoomsByHotelIdAsync(hotelId);
            var roomsViewModel = roomsDTO.Select(hd => _mapper.Map<RoomViewModel>(hd));
            return Ok(roomsViewModel);
        }

        [HttpGet("{hotelId:int}/{roomId:int}")]
        public async Task<IActionResult> GetById(int hotelId, int roomId, CancellationToken ct = default)
        {
            var roomDTO = await _roomService.GetRoomByIdAsync(hotelId, roomId, ct);
            var roomViewModel = _mapper.Map<RoomViewModel>(roomDTO);
            return Ok(roomViewModel);
        }
    }
}
