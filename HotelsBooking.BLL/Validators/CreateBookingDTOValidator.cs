
using FluentValidation;
using HotelsBooking.BLL.DTO;

namespace HotelsBooking.BLL.Validators
{
    public class CreateBookingDTOValidator : AbstractValidator<CreateBookingDTO>
    {
        public CreateBookingDTOValidator()
        {
            RuleFor(b => b.RoomId).GreaterThan(0);
            RuleFor(b => b.CheckInDate)
                .Must(d => d.Date >= DateTime.UtcNow.Date)
                .WithMessage("Дата заезда не может быть в прошлом.");

            RuleFor(b => b.CheckOutDate)
                .GreaterThan(b => b.CheckInDate)
                .WithMessage("Дата выезда должна быть позже даты заезда.")
                .Must((dto, checkout) =>
                (checkout - dto.CheckInDate).TotalDays % 1 == 0)
                .WithMessage("Разница между датами должна быть кратна дню."); ;

            RuleFor(b => b.Adults).GreaterThan(0);
            RuleFor(b => b.Children).GreaterThanOrEqualTo(0);
        }
    }

}
