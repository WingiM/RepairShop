using FluentValidation;

namespace RepairShop.Validation;

public class UpdateRepairRequestValidator : AbstractValidator<UpdateRepairRequestDto>
{
    public UpdateRepairRequestValidator(ApplicationContext context)
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

        RuleFor(x => x.RequestId)
            .Must(x => context.RepairRequests.Find(x) is not null)
            .WithMessage(ValidationErrorMessages.RepairRequestDoesNotExist)
            .Must(x => context.RepairRequests.Find(x)!.MasterId == null)
            .WithMessage(ValidationErrorMessages.CannotEditRepairRequest);

    }
}
