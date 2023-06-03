using FluentValidation;
using RepairShop.Data;
using RepairShop.Data.DTO;
using RepairShop.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairShop.Validation;

public class UpdateRepairRequestValidator : AbstractValidator<UpdateRepairRequestDto>
{
    public UpdateRepairRequestValidator(ApplicationContext context)
    {
        RuleFor(x => x.Description)
            .NotNull()
            .NotEmpty()
            .MaximumLength(1000);

        RuleFor(x => x.ShortName)
            .NotNull()
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.RequestId)
            .Must(x => context.RepairRequests.Find(x) is not null)
            .WithMessage("Указанного заказа не существует в системе")
            .Must(x => context.RepairRequests.Find(x)!.MasterId == null)
            .WithMessage("Нельзя отредактировать запрос на ремонт, когда он передан мастеру");

    }
}
