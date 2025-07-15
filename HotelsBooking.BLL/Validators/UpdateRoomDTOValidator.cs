
using FluentValidation;
using HotelsBooking.BLL.DTO;
using HotelsBooking.DAL.Constants;

namespace HotelsBooking.BLL.Validators
{
    public class UpdateRoomDTOValidator: AbstractValidator<UpdateRoomDTO>
    {
        public UpdateRoomDTOValidator()
        {
            RuleFor(r => r.Type)
                .Must(type => Enum.TryParse<RoomType>(type, out var parsedType))
                .WithMessage("Недопустимый тип номера отеля.");

            RuleFor(x => x.PricePerNight)
                .GreaterThan(0).WithMessage("Цена должна быть положительной.");

            RuleFor(x => x.Capacity)
                .GreaterThan(0).WithMessage("Вместимость должна быть положительной.");
        }
    }
}
