namespace RepairShop.Data.DTO;

public class UpdateRepairRequestDto
{
    public required int RequestId { get; init; }

    public required string ShortName { get; init; }

    public required string Description { get; init; }
}
