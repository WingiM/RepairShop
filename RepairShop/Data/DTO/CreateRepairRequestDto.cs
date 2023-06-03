namespace RepairShop.Data.DTO;

public class CreateRepairRequestDto
{
    public int ClientId { get; set; }

    public string ShortName { get; set; } = null!;

    public string Description { get; set; } = null!;
}
