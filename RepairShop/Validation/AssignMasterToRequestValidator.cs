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

public class AssignMasterToRequestValidator : AbstractValidator<AssignMasterToRequestDto>
{
    public AssignMasterToRequestValidator(ApplicationContext context)
    {
        RuleFor(x => x.MasterId)
            .Must(x => context.Users.Find(x) != null)
            .WithMessage("Указанного пользователя не сущетсвует в системе")
            .Must(x => context.Users.Find(x)!.RoleId == (int)Roles.Master)
            .WithMessage("Указанный пользователь не является мастером");

        RuleFor(x => x.RequestId)
            .Must(x => context.RepairRequests.Find(x) != null)
            .WithMessage("Такого запроса на ремонт не существует")
            .Must(x => context.StatusHistories.First(z => z.Id == x && z.IsActual).StatusId == (int)RequestStatuses.AwaitsConfirmation)
            .WithMessage("Запрос на ремонт уже переведен в другой статус");
    }
}
