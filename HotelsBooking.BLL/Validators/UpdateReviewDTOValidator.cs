
using FluentValidation;
using HotelsBooking.BLL.DTO;

namespace HotelsBooking.BLL.Validators
{
    public class UpdateReviewDTOValidator : AbstractValidator<UpdateReviewDTO>
    {
        public UpdateReviewDTOValidator()
        {
            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5).WithMessage("Оценка должна быть от 1 до 5.");

            RuleFor(x => x.Comment)
                .NotEmpty().WithMessage("Комментарий обязателен.")
                .MaximumLength(1000).WithMessage("Комментарий не должен превышать 1000 символов.");
        }
    }
}
