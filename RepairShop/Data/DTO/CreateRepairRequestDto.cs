namespace RepairShop.Data.DTO;

public class CreateRepairRequestDto
{
    public required int ClientId { get; init; }

    public required string ShortName { get; init; }

    public required string Description { get; init; }
}
