using AutoMapper;
using HotelsBooking.API.Adapters;
using HotelsBooking.API.Constants;
using HotelsBooking.API.Models;
using HotelsBooking.API.ViewModels;
using HotelsBooking.BLL.DTO;
using HotelsBooking.BLL.Services;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("hotel/{hotelId:int}")]
        public async Task<IActionResult> GetAll(int hotelId, CancellationToken ct = default) //rename
        {
            var roomsDTO = await _roomService.GetRoomsByHotelIdAsync(hotelId);
            var roomsViewModel = roomsDTO.Select(hd => _mapper.Map<RoomViewModel>(hd));
            return Ok(roomsViewModel);
        }

        [HttpGet("{roomId:int}")]
        public async Task<IActionResult> GetById(int roomId, CancellationToken ct = default)
        {
            var roomDTO = await _roomService.GetRoomByIdAsync(roomId, ct);
            var roomViewModel = _mapper.Map<RoomViewModel>(roomDTO);
            return Ok(roomViewModel);
        }

        [Authorize(Policy = Policies.HotelOwner)]
        [HttpPost("{id:int}/photo")]
        public async Task<IActionResult> UploadRoomPhoto(int id, IFormFile photo, CancellationToken ct = default)
        {
            var adapter = new FormFileAdapter(photo);
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            await _roomService.UploadRoomPhotoAsync(id, adapter, userEmail, ct);
            return Ok();
        }

        [Authorize(Policy = Policies.HotelOwner)]
        [HttpDelete("{roomId:int}/photo/{photoId:int}")]
        public async Task<IActionResult> DeleteRoomPhoto(int roomId, int photoId, CancellationToken ct = default)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            await _roomService.DeleteRoomPhotoAsync(roomId, photoId, userEmail, ct);
            return NoContent();
        }

        [Authorize(Policy = Policies.HotelOwner)]
        [HttpDelete("{roomId:int}")]
        public async Task<IActionResult> Delete(int roomId, CancellationToken ct = default)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            await _roomService.DeleteRoomAsync(roomId, userEmail, ct);
            return NoContent();
        }

        [Authorize(Policy = Policies.HotelOwner)]
        [HttpPut("{roomId:int}")]
        public async Task<IActionResult> Update(int roomId, UpdateRoomModel updatingRoom, CancellationToken ct = default)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var updatingRoomDTO = _mapper.Map<UpdateRoomDTO>(updatingRoom);
            await _roomService.UpdateRoomAsync(roomId, userEmail, updatingRoomDTO, ct);
            return Ok();
        }
    }
}
