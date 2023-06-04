namespace RepairShop.Data.DTO;

public class CreateRepairRequestDto
{
    public required int ClientId { get; set; }

    public required string ShortName { get; set; }

    public required string Description { get; set; }
}
