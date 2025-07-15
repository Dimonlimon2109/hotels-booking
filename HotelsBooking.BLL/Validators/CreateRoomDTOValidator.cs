
using FluentValidation;
using HotelsBooking.BLL.DTO;
using HotelsBooking.DAL.Constants;

namespace HotelsBooking.BLL.Validators
{
    public class CreateRoomDTOValidator : AbstractValidator<CreateRoomDTO>
    {
        public CreateRoomDTOValidator()
        {
            RuleFor(r => r.HotelId)
                .GreaterThan(0).WithMessage("HotelId должен быть положительным числом.");


            RuleFor(r => r.Type)
                .Must(type => Enum.TryParse<RoomType>(type, out var parsedType))
                .WithMessage("Недопустимый тип номера отеля.");

            RuleFor(r => r.PricePerNight)
                .GreaterThan(0).WithMessage("Цена должна быть положительной.");

            RuleFor(r => r.Capacity)
                .GreaterThan(0).WithMessage("Вместимость должна быть положительной.");
        }
    }
}
