using LanguageExt.Common;
using RepairShop.Data.Enums;
using RepairShop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairShop.Services;

public interface IRequestService
{
    public Result<IEnumerable<RepairRequest>> ListForMaster(int masterId);
    public Result<IEnumerable<RepairRequest>> ListForClient(int clientId);
    public Result<RepairRequest> Create(int clientId, string shortName, string description);
    public Result<RepairRequest> Update(int requestId, RepairRequest request);
    public Result<bool> AssignMaster(int requestId, int masterId);
    public Result<bool> ChangeStatus(int requestId, RequestStatuses status);
}
