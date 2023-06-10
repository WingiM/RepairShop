using FluentValidation;

namespace RepairShop.Validation;

public class CreateRepairRequestValidator : AbstractValidator<CreateRepairRequestDto>
{
    public CreateRepairRequestValidator(ApplicationContext context)
    {
        RuleFor(x => x.Description)
            .NotNull()
            .WithMessage(ValidationErrorMessages.RepairRequestDescriptionCannotBeEmpty)
            .NotEmpty()
            .WithMessage(ValidationErrorMessages.RepairRequestDescriptionCannotBeEmpty)
            .MaximumLength(1000)
            .WithMessage(ValidationErrorMessages.RepairRequestDescriptionCannotBeMoreThan1000Symbols);

        RuleFor(x => x.ShortName)
            .NotNull()
            .WithMessage(ValidationErrorMessages.RepairRequestNameCannotBeEmpty)
            .NotEmpty()
            .WithMessage(ValidationErrorMessages.RepairRequestNameCannotBeEmpty)
            .MaximumLength(50)
            .WithMessage(ValidationErrorMessages.RepairRequestNameCannotBeMoreThan50Symbols);

        RuleFor(x => x.ClientId)
            .NotNull()
            .Must(x => context.Users.Any(z => z.Id == x && z.RoleId == (int)Roles.Client))
            .WithMessage(ValidationErrorMessages.ClientDoesNotExist);
    }
}
