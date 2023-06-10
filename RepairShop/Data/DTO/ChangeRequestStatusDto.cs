namespace RepairShop.Data.DTO;

public class ChangeRequestStatusDto
{
    public required int RequestId { get; init; }
    public required RequestStatuses Status { get; init; }
}
