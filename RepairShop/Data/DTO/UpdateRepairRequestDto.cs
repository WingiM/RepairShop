namespace RepairShop.Data.DTO;

public class UpdateRepairRequestDto
{
    public required int RequestId { get; set; }

    public required string ShortName { get; set; }

    public required string Description { get; set; }
}
