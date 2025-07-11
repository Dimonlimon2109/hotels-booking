using AutoMapper;
using HotelsBooking.API.Models;
using HotelsBooking.API.ViewModels;
using HotelsBooking.BLL.DTO;
using HotelsBooking.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelsBooking.API.Controllers
{
    [Route("api/amenities")]
    [ApiController]
    public class AmenityController : ControllerBase
    {
        private readonly AmenityService _amenityService;
        private readonly IMapper _mapper;

        public AmenityController(AmenityService amenityService, IMapper mapper)
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

        [HttpPost]
        public async Task<IActionResult> Create(CreateAmenityModel creatingAmenity, CancellationToken ct = default)
        {
            var creatingAmenityDTO = _mapper.Map<CreateAmenityDTO>(creatingAmenity);
            await _amenityService.CreateAmenityAsync(creatingAmenityDTO, ct);
            return Created();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct = default)
        {
            await _amenityService.DeleteAmenityAsync(id, ct);
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateAmenityModel updatingAmenity, CancellationToken ct = default)
        {
            var updatingAmenityDTO = _mapper.Map<UpdateAmenityDTO>(updatingAmenity);
            await _amenityService.UpdateAmenityAsync(id, updatingAmenityDTO, ct);
            return Ok();
        }
    }
}
