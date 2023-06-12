using LanguageExt.Common;

namespace RepairShop.Services;

public interface IAuthorizationService
{
    public Result<User> AuthorizeUser(string login, string password);
    public Result<User> RegisterClient(RegisterUserDto user);
    public Result<bool> ChangePassword(ChangeUserPasswordDto dto);
}
