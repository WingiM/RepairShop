using FluentValidation;

namespace RepairShop.Validation;

public class ChangeRequestStatusValidator : AbstractValidator<ChangeRequestStatusDto>
{
    public ChangeRequestStatusValidator(ApplicationContext context)
    {
        RuleFor(x => x.Status)
            .Must(x => (int)x >= (int)RequestStatuses.TransferedToMaster)
            .WithMessage(ValidationErrorMessages.CannotChangeRequestStatus);

        RuleFor(x => x.RequestId)
            .Must(x => context.RepairRequests.Find(x) is not null)
            .WithMessage(ValidationErrorMessages.RepairRequestDoesNotExist)
            .Must(x => context.StatusHistories.First(z => z.RequestId == x && z.IsActual).StatusId >= (int)RequestStatuses.TransferedToMaster)
            .WithMessage(ValidationErrorMessages.CannotChangeRequestStatus);

        RuleFor(x => new { x.Status, x.RequestId })
            .Must(x => context.StatusHistories.First(z => z.IsActual && z.RequestId == x.RequestId).StatusId != (int)x.Status)
            .WithMessage(ValidationErrorMessages.CannotChangeToSameStatus);
    }
}
