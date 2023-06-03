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

public class ChangeRequestStatusValidator : AbstractValidator<ChangeRequestStatusDto>
{
    public ChangeRequestStatusValidator(ApplicationContext context)
    {
        RuleFor(x => x.Status)
            .Must(x => (int)x > (int)RequestStatuses.TransferedToMaster)
            .WithMessage("Можно изменять статусы только для уже переданных мастеру запросов на ремонт");

        RuleFor(x => x.RequestId)
            .Must(x => context.RepairRequests.Find(x) is not null)
            .WithMessage("Указанного запроса на ремонт не существует")
            .Must(x => context.StatusHistories.First(z => z.RequestId == x && z.IsActual).StatusId >= (int)RequestStatuses.TransferedToMaster)
            .WithMessage("Можно изменять статусы только для уже переданных мастеру запросов на ремонт");
    }
}
