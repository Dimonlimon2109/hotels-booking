using FluentValidation;
using HotelsBooking.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelsBooking.BLL.Validators
{
    public class LoginDTOValidator: AbstractValidator<LoginDTO>
    {
        public LoginDTOValidator()
        {
            RuleFor(r => r.Email)
                .EmailAddress()
                .WithMessage("Некорректный email адрес");

            RuleFor(r => r.Password)
                .MinimumLength(8)
                .WithMessage("Длина пароля должна составлть от 8 до 30 символов")
                .MaximumLength(30)
                .WithMessage("Длина пароля должна составлть от 8 до 30 символов")
                .Matches(@"^(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+{}\[\]:;<>,.?~\\/-]).*$")
                .WithMessage("Пароль должен содержать минимум одну заглавную букву, цифру и спецсимвол");
        }
    }
}
