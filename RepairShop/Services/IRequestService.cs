using LanguageExt.Common;
using RepairShop.Data.DTO;
using RepairShop.Data.Models;
using System.Collections.Generic;

namespace RepairShop.Services;

public interface IRequestService
{
    public Result<IEnumerable<RepairRequest>> ListForMaster(int masterId);
    public Result<IEnumerable<RepairRequest>> ListForClient(int clientId);
    public Result<RepairRequest?> GetRequestById(int requestId);
    public Result<IEnumerable<StatusHistory>> GetStatusHistoryForRequest(int requestId);
    public Result<RepairRequest> Create(CreateRepairRequestDto request);
    public Result<RepairRequest> Update(UpdateRepairRequestDto request);
    public Result<bool> AssignMaster(AssignMasterToRequestDto request);
    public Result<bool> ChangeStatus(ChangeRequestStatusDto request);
}
