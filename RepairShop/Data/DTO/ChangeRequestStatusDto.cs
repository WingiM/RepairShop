using RepairShop.Data.Enums;

namespace RepairShop.Data.DTO;

public class ChangeRequestStatusDto
{
    public required int RequestId { get; set; }
    public required RequestStatuses Status { get; set; }
}
