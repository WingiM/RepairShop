using LanguageExt.Common;

namespace RepairShop.Services;

public interface IAuthorizationService
{
    public Result<bool> AuthorizeUser(string login, string password);
}
