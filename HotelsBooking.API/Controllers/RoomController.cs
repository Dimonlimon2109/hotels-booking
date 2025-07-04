using AutoMapper;
using HotelsBooking.API.Models;
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
        public async Task<IActionResult> CreateRoom(CreateRoomModel creatingRoom, CancellationToken ct = default)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var createRoomDTO = _mapper.Map<CreateRoomDTO>(creatingRoom);
            var RoomDTO = await _roomService.CreateRoomAsync(userEmail, createRoomDTO, ct);
            return Created();
        }
    }
}
