
using FluentValidation;
using HotelsBooking.BLL.DTO;

namespace HotelsBooking.BLL.Validators
{
    public class CreateHotelDTOValidator : AbstractValidator<CreateHotelDTO>
    {
        public CreateHotelDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Название отеля обязательно.")
                .MaximumLength(100).WithMessage("Название отеля не должно превышать 100 символов.");

            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("Страна обязательна.")
                .MaximumLength(100).WithMessage("Название страны не должно превышать 100 символов.");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("Город обязателен.")
                .MaximumLength(100).WithMessage("Название города не должно превышать 100 символов.");

            RuleFor(x => x.Street)
                .NotEmpty().WithMessage("Улица обязательна.")
                .MaximumLength(150).WithMessage("Название улицы не должно превышать 150 символов.");

            RuleFor(x => x.HouseNumber)
                .NotEmpty().WithMessage("Номер дома обязателен.")
                .MaximumLength(20).WithMessage("Номер дома не должен превышать 20 символов.");

            RuleFor(x => x.StarRating)
                .InclusiveBetween(1, 5).WithMessage("Звкзды должны быть от 1 до 5.");

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
