
using FluentValidation;
using HotelsBooking.BLL.DTO;

namespace HotelsBooking.BLL.Validators
{
    public class UpdateRoomDTOValidator: AbstractValidator<UpdateRoomDTO>
    {
        public UpdateRoomDTOValidator()
        {
            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("Тип номера обязателен.")
                .Length(3, 50).WithMessage("Тип номера должен содержать от 3 до 50 символов.");

            RuleFor(x => x.PricePerNight)
                .GreaterThan(0).WithMessage("Цена должна быть положительной.");

            RuleFor(x => x.Capacity)
                .GreaterThan(0).WithMessage("Вместимость должна быть положительной.");
        }
    }
}
