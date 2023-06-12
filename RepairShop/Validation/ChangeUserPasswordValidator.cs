using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairShop.Validation;

public class ChangeUserPasswordValidator : AbstractValidator<ChangeUserPasswordDto>
{
    public ChangeUserPasswordValidator(ApplicationContext context)
    {
        RuleFor(x => x.NewPassword)
           .NotEmpty()
           .WithMessage(ValidationErrorMessages.PasswordCannotBeEmpty)
           .MaximumLength(50)
           .WithMessage(ValidationErrorMessages.PasswordCannotBeMoreThan50Symbols);

        RuleFor(x => new { x.Login, x.OldPassword })
            .Must(x => context.Users.FirstOrDefault(z => z.Login == x.Login && z.Password == x.OldPassword) is not null)
            .WithMessage(ValidationErrorMessages.WrongOldPassword);
    }
}
