using LanguageExt.Common;
using RepairShop.Data.Models;

namespace RepairShop.Services;

public interface IAuthorizationService
{
    public Result<User> AuthorizeUser(string login, string password);
}
