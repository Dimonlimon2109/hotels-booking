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
    [Route("api/rooms")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;
        private readonly IBookingService _bookingService;
        private readonly IMapper _mapper;

        public RoomController(IRoomService roomService, IBookingService bookingService, IMapper mapper)
        {
            _roomService = roomService;
            _bookingService = bookingService;
            _mapper = mapper;
        }

        [Authorize(Roles = Roles.HotelOwner)]
        [HttpPost]
        public async Task<IActionResult> Create(CreateRoomModel creatingRoom, CancellationToken ct = default)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var createRoomDTO = _mapper.Map<CreateRoomDTO>(creatingRoom);
            var RoomDTO = await _roomService.CreateRoomAsync(userEmail, createRoomDTO, ct);
            return Created();
        }

        [HttpGet("{roomId:int}")]
        public async Task<IActionResult> GetById(int roomId, CancellationToken ct = default)
        {
            var roomDTO = await _roomService.GetRoomByIdAsync(roomId, ct);
            var roomViewModel = _mapper.Map<RoomViewModel>(roomDTO);
            return Ok(roomViewModel);
        }

        [Authorize(Roles = Roles.HotelOwner)]
        [HttpPost("{id:int}/photo")]
        public async Task<IActionResult> UploadRoomPhoto(int id, IFormFile photo, CancellationToken ct = default)
        {
            var adapter = new FormFileAdapter(photo);
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            await _roomService.UploadRoomPhotoAsync(id, adapter, userEmail, ct);
            return Ok();
        }

        [Authorize(Roles = Roles.HotelOwner)]
        [HttpDelete("{roomId:int}/photo/{photoId:int}")]
        public async Task<IActionResult> DeleteRoomPhoto(int roomId, int photoId, CancellationToken ct = default)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            await _roomService.DeleteRoomPhotoAsync(roomId, photoId, userEmail, ct);
            return NoContent();
        }

        [Authorize(Roles = Roles.HotelOwner)]
        [HttpDelete("{roomId:int}")]
        public async Task<IActionResult> Delete(int roomId, CancellationToken ct = default)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            await _roomService.DeleteRoomAsync(roomId, userEmail, ct);
            return NoContent();
        }

        [Authorize(Roles = Roles.HotelOwner)]
        [HttpPut("{roomId:int}")]
        public async Task<IActionResult> Update(int roomId, UpdateRoomModel updatingRoom, CancellationToken ct = default)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var updatingRoomDTO = _mapper.Map<UpdateRoomDTO>(updatingRoom);
            await _roomService.UpdateRoomAsync(roomId, userEmail, updatingRoomDTO, ct);
            return Ok();
        }

        [Authorize(Roles = Roles.HotelOwner)]
        [HttpGet("{roomId}/bookings")]
        public async Task<IActionResult> GetBookingsByRoomId (int roomId, CancellationToken ct = default)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var bookingsDTO = await _bookingService.GetBookingsByRoomAsync(roomId, userEmail, ct);

            return Ok(bookingsDTO.Select(bd => _mapper.Map<BookingViewModel>(bd)));
        }
    }
}
