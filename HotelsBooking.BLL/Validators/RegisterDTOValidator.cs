
using FluentValidation;
using HotelsBooking.BLL.DTO;
using HotelsBooking.DAL.Constants;

namespace HotelsBooking.BLL.Validators
{
    public class RegisterDTOValidator : AbstractValidator<RegisterDTO>
    {
        public RegisterDTOValidator()
        {
            RuleFor(r => r.UserName)
                .MinimumLength(3)
                .WithMessage("Длина имени пользователя должна составлять от 3 до 50 символов")
                .MaximumLength(50)
                .WithMessage("Длина имени пользователя должна составлять от 3 до 50 символов");

            RuleFor(r => r.Password)
                .MinimumLength(8)
                .WithMessage("Длина пароля должна составлть от 8 до 30 символов")
                .MaximumLength(30)
                .WithMessage("Длина пароля должна составлть от 8 до 30 символов")
                .Matches(@"^(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+{}\[\]:;<>,.?~\\/-]).*$")
                .WithMessage("Пароль должен содержать минимум одну заглавную букву, цифру и спецсимвол");

            RuleFor(r => r.Email)
                .EmailAddress()
                .WithMessage("Некорректный email адрес");

            RuleFor(r => r.Role)
                .Must(role => Enum.TryParse<UserRole>(role, out var parsedRole)
                            && parsedRole != UserRole.Admin)
                .WithMessage("Допустимые роли: HotelOwner, Guest");
        }
    }
}
