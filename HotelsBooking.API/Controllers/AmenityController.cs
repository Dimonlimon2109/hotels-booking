using AutoMapper;
using HotelsBooking.API.Constants;
using HotelsBooking.API.Models;
using HotelsBooking.API.ViewModels;
using HotelsBooking.BLL.DTO;
using HotelsBooking.BLL.Interfaces;
using HotelsBooking.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelsBooking.API.Controllers
{
    [Route("api/amenities")]
    [ApiController]
    public class AmenityController : ControllerBase
    {
        private readonly IAmenityService _amenityService;
        private readonly IMapper _mapper;

        public AmenityController(IAmenityService amenityService, IMapper mapper)
        {
            _amenityService = amenityService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct = default)
        {
            var amenitiesDTO = await _amenityService.GetAllAmenitiesAsync(ct);
            return Ok(amenitiesDTO.Select(ad => _mapper.Map<AmenityViewModel>(ad)));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct = default)
        {
            var amenityDTO = await _amenityService.GetAmenityByIdAsync(id, ct);
            return Ok(_mapper.Map<AmenityViewModel>(amenityDTO));
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        public async Task<IActionResult> Create(CreateAmenityModel creatingAmenity, CancellationToken ct = default)
        {
            var creatingAmenityDTO = _mapper.Map<CreateAmenityDTO>(creatingAmenity);
            await _amenityService.CreateAmenityAsync(creatingAmenityDTO, ct);
            return Created();
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct = default)
        {
            await _amenityService.DeleteAmenityAsync(id, ct);
            return NoContent();
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateAmenityModel updatingAmenity, CancellationToken ct = default)
        {
            var updatingAmenityDTO = _mapper.Map<UpdateAmenityDTO>(updatingAmenity);
            await _amenityService.UpdateAmenityAsync(id, updatingAmenityDTO, ct);
            return Ok();
        }
    }
}
