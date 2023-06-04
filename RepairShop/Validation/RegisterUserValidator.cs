using FluentValidation;
using RepairShop.Data;
using RepairShop.Data.DTO;
using System.Linq;

namespace RepairShop.Validation;

public class RegisterUserValidator : AbstractValidator<RegisterUserDto>
{
    public RegisterUserValidator(ApplicationContext context)
    {
        RuleFor(x => x.Login)
            .NotEmpty()
            .MaximumLength(50)
            .Must(x => context.Users.FirstOrDefault(z => z.Login == x) is null)
            .WithMessage("Пользователь с таким логином уже существует");

        RuleFor(x => x.Password)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => new { x.Password, x.RepeatPassword })
            .Must(x => x.Password == x.RepeatPassword)
            .WithMessage("Пароли не совпадают");
    }
}
