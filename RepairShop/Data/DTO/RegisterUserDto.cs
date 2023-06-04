namespace RepairShop.Data.DTO;

public class RegisterUserDto
{
    public required string Login { get; init; }
    public required string Password { get; init; }
    public required string RepeatPassword { get; init; }
}
