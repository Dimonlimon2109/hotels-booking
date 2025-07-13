
using FluentValidation;
using HotelsBooking.BLL.DTO;
using HotelsBooking.DAL.Constants;

namespace HotelsBooking.BLL.Validators
{
    public class UpdateBookingStatusDTOValidator : AbstractValidator<UpdateBookingStatusDTO>
    {
        public UpdateBookingStatusDTOValidator()
        {
            RuleFor(b => b.Status)
                .Must(status => Enum.TryParse<BookingStatus>(status, out var parsedStatus))
                .WithMessage("Недопустимый статус бронирования.");
        }
    }
}
