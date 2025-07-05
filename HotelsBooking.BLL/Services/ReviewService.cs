
using AutoMapper;
using FluentValidation;
using HotelsBooking.BLL.DTO;
using HotelsBooking.DAL.Entities;
using HotelsBooking.DAL.Interfaces;
using HotelsBooking.DAL.Repositories;
using System.Security;

namespace HotelsBooking.BLL.Services
{
    public class ReviewService
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IUserRepository _userRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateReviewDTO> _creatingReviewValidator;
        private readonly IValidator<UpdateReviewDTO> _updatingReviewValidator;

        public ReviewService
            (
            IHotelRepository hotelRepository,
            IUserRepository userRepository,
            IReviewRepository reviewRepository,
            IMapper mapper,
            IValidator<CreateReviewDTO>
            creatingReviewValidator,
            IValidator<UpdateReviewDTO> updatingReviewValidator
            )
        {
            _hotelRepository = hotelRepository;
            _userRepository = userRepository;
            _reviewRepository = reviewRepository;
            _mapper = mapper;
            _creatingReviewValidator = creatingReviewValidator;
            _updatingReviewValidator = updatingReviewValidator;
        }

        public async Task CreateReviewAsync(CreateReviewDTO creatingReview, string userEmail, CancellationToken ct = default)
        {
            var validationResult = await _creatingReviewValidator.ValidateAsync(creatingReview, ct);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var hotel = await _hotelRepository.GetByIdAsync(creatingReview.HotelId)
                ?? throw new NullReferenceException("Отель не найден");

            var user = await _userRepository.GetByEmailAsync(userEmail, ct)
                ?? throw new NullReferenceException("Пользователь не найден");

            var review = _mapper.Map<Review>(creatingReview);
            review.UserId = user.Id;
            await _reviewRepository.AddAsync(review, ct);
        }

        public async Task<IEnumerable<ReviewDTO>> GetAllReviewsByHotelAsync(int hotelId, CancellationToken ct = default)
        {
            var hotel = await _hotelRepository.GetByIdAsync(hotelId)
                ?? throw new NullReferenceException("Отель не найден");

            var reviews = await _reviewRepository.GetReviewsWithUserAsync(hotelId, ct);
            return reviews.Select(a => _mapper.Map<ReviewDTO>(a));
        }

        public async Task DeleteReviewAsync(int id, string userEmail, CancellationToken ct = default)
        {
            var user = await _userRepository.GetByEmailAsync(userEmail, ct)
                ?? throw new NullReferenceException("Пользователь не найден");

            var review = await _reviewRepository.GetByIdAsync(id, ct)
                ?? throw new NullReferenceException("Отзыв не найден");

            if (review.UserId != user.Id)
            {
                throw new SecurityException("Только пользователь, оставивший отзыв, может его удалить");
            }
            await _reviewRepository.DeleteAsync(id, ct);
        }

        public async Task UpdateReviewAsync(int id, UpdateReviewDTO updatingReview, string userEmail, CancellationToken ct = default)
        {
            var validationResult = await _updatingReviewValidator.ValidateAsync(updatingReview, ct);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var user = await _userRepository.GetByEmailAsync(userEmail, ct)
                ?? throw new NullReferenceException("Пользователь не найден");

            var review = await _reviewRepository.GetByIdAsync(id, ct)
                ?? throw new NullReferenceException("Отзыв не найден");

            if(review.UserId != user.Id)
            {
                throw new SecurityException("Только пользователь, оставивший отзыв, может его изменить");
            }

            review.Comment = updatingReview.Comment;
            review.Rating = updatingReview.Rating;

            await _reviewRepository.UpdateAsync(review, ct);
        }
    }
}
