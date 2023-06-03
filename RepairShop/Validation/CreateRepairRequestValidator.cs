using FluentValidation;
using RepairShop.Data;
using RepairShop.Data.DTO;
using RepairShop.Data.Enums;
using System.Linq;

namespace RepairShop.Validation;

public class CreateRepairRequestValidator : AbstractValidator<CreateRepairRequestDto>
{
    public CreateRepairRequestValidator(ApplicationContext context)
    {
        RuleFor(x => x.Description)
            .NotNull()
            .NotEmpty()
            .MaximumLength(1000);

        RuleFor(x => x.ShortName)
            .NotNull()
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.ClientId)
            .NotNull()
            .Must(x => context.Users.Any(z => z.Id == x && z.RoleId == (int)Roles.Client))
            .WithMessage("Такого клиента нет в системе");
    }
}
