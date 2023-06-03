using LanguageExt.Common;
using RepairShop.Data.DTO;
using RepairShop.Data.Enums;
using RepairShop.Data.Models;
using System.Collections.Generic;

namespace RepairShop.Services;

public interface IRequestService
{
    public Result<IEnumerable<RepairRequest>> ListForMaster(int masterId);
    public Result<IEnumerable<RepairRequest>> ListForClient(int clientId);
    public Result<RepairRequest> Create(CreateRepairRequestDto request);
    public Result<RepairRequest> Update(UpdateRepairRequestDto request);
    public Result<bool> AssignMaster(int requestId, int masterId);
    public Result<bool> ChangeStatus(int requestId, RequestStatuses status);
}
