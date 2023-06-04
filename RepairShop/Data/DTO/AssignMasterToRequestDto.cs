namespace RepairShop.Data.DTO;

public class AssignMasterToRequestDto
{
    public required int RequestId { get; init; }
    
    public required int MasterId { get; init; }
}
