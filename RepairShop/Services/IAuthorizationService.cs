using LanguageExt.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairShop.Services;

public interface IAuthorizationService
{
    public Result<bool> AuthorizeUser(string login, string password);
}
