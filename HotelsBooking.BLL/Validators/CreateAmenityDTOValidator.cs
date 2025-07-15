using FluentValidation;
using HotelsBooking.BLL.DTO;

namespace HotelsBooking.BLL.Validators
{
    public class CreateAmenityDTOValidator : AbstractValidator<CreateAmenityDTO>
    {
        public CreateAmenityDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Название удобства обязательно.")
                .MaximumLength(100).WithMessage("Название удобства не должно превышать 100 символов.");
        }
    }
}
