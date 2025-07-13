using AutoMapper;
using HotelsBooking.API.Constants;
using HotelsBooking.API.Models;
using HotelsBooking.API.ViewModels;
using HotelsBooking.BLL.DTO;
using HotelsBooking.BLL.Interfaces;
using HotelsBooking.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotelsBooking.API.Controllers
{
    [Route("api/reviews")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly IMapper _mapper;

        public ReviewController(IReviewService reviewService, IMapper mapper)
        {
            _reviewService = reviewService;
            _mapper = mapper;
        }

        [HttpGet("hotels/{hotelId:int}")]
        public async Task<IActionResult> GetByHotel(int hotelId, CancellationToken ct = default)
        {
            var reviewsDTO = await _reviewService.GetAllReviewsByHotelAsync(hotelId, ct);
            return Ok(reviewsDTO.Select(rd => _mapper.Map<ReviewViewModel>(rd)));
        }

        [HttpPost]
        [Authorize(Policy = Policies.Client)]
        public async Task<IActionResult> Create(CreateReviewModel creatingReview, CancellationToken ct = default)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var creatingReviewDTO = _mapper.Map<CreateReviewDTO>(creatingReview);
            await _reviewService.CreateReviewAsync(creatingReviewDTO, userEmail, ct);
            return Created();
        }

        [Authorize(Policy = Policies.Client)]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct = default)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            await _reviewService.DeleteReviewAsync(id, userEmail, ct);
            return NoContent();
        }

        [Authorize(Policy = Policies.Client)]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateReviewModel updatingReview, CancellationToken ct = default)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var updatingReviewDTO = _mapper.Map<UpdateReviewDTO>(updatingReview);
            await _reviewService.UpdateReviewAsync(id, updatingReviewDTO, userEmail, ct);
            return Ok();
        }
    }
}
