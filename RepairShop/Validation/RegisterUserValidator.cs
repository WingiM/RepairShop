using FluentValidation;

namespace RepairShop.Validation;

public class RegisterUserValidator : AbstractValidator<RegisterUserDto>
{
    public RegisterUserValidator(ApplicationContext context)
    {
        RuleFor(x => x.Login)
            .NotEmpty()
            .WithMessage(ValidationErrorMessages.LoginCannotBeEmpty)
            .MaximumLength(50)
            .WithMessage(ValidationErrorMessages.LoginCannotBeMoreThan50Symbols)
            .Must(x => context.Users.FirstOrDefault(z => z.Login == x) is null)
            .WithMessage(ValidationErrorMessages.UserAlreadyExists);

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage(ValidationErrorMessages.PasswordCannotBeEmpty)
            .MaximumLength(50)
            .WithMessage(ValidationErrorMessages.PasswordCannotBeMoreThan50Symbols);

        RuleFor(x => new { x.Password, x.RepeatPassword })
            .Must(x => x.Password == x.RepeatPassword)
            .WithMessage(ValidationErrorMessages.PasswordsDoNotMatch);
    }
}
