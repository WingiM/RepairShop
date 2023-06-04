using LanguageExt.Common;
using RepairShop.Data.DTO;
using RepairShop.Data.Models;

namespace RepairShop.Services;

public interface IAuthorizationService
{
    public Result<User> AuthorizeUser(string login, string password);
    public Result<User> RegisterClient(RegisterUserDto user);
}
