namespace RepairShop.Data.DTO;

public class ChangeUserPasswordDto
{
    public required string Login { get; init; }
    public required string OldPassword { get; init; }
    public required string NewPassword { get; init; }
}
