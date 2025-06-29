using FluentValidation;
using HotelsBooking.BLL.DTO;

namespace HotelsBooking.BLL.Validators
{
    public class UpdateHotelDTOValidator : AbstractValidator<UpdateHotelDTO>
    {
        public UpdateHotelDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Название отеля обязательно.")
                .MaximumLength(100).WithMessage("Название отеля не должно превышать 100 символов.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Адрес обязателен.")
                .MaximumLength(200).WithMessage("Адрес не должен превышать 200 символов.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Описание обязательно.")
                .MaximumLength(1000).WithMessage("Описание не должно превышать 1000 символов.");

            RuleFor(x => x.Latitude)
                .InclusiveBetween(-90, 90).WithMessage("Широта должна быть в диапазоне от -90 до 90.");

            RuleFor(x => x.Longitude)
                .InclusiveBetween(-180, 180).WithMessage("Долгота должна быть в диапазоне от -180 до 180.");
        }
    }
}
