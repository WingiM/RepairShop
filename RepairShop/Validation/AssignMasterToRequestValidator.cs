using FluentValidation;

namespace RepairShop.Validation;

public class AssignMasterToRequestValidator : AbstractValidator<AssignMasterToRequestDto>
{
    public AssignMasterToRequestValidator(ApplicationContext context)
    {
        RuleFor(x => x.MasterId)
            .Must(x => context.Users.Find(x) != null)
            .WithMessage(ValidationErrorMessages.UserDoesNotExist)
            .Must(x => context.Users.Find(x)!.RoleId == (int)Roles.Master)
            .WithMessage(ValidationErrorMessages.UserIsNotMaster);

        RuleFor(x => x.RequestId)
            .Must(x => context.RepairRequests.Find(x) != null)
            .WithMessage(ValidationErrorMessages.RepairRequestDoesNotExist)
            .Must(x => context.StatusHistories.First(z => z.Id == x && z.IsActual).StatusId ==
                       (int)RequestStatuses.AwaitsConfirmation)
            .WithMessage(ValidationErrorMessages.RepairRequestStatusAlreadyChanged);
    }
}