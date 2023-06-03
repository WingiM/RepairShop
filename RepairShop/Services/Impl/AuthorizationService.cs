using LanguageExt.Common;
using RepairShop.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairShop.Services.Impl;

public class AuthorizationService : IAuthorizationService
{
    private readonly ApplicationContext _context;

    public AuthorizationService(ApplicationContext context)
    {
        _context = context;
    }

    public Result<bool> AuthorizeUser(string login, string password)
    {
        var userExists = _context.Users.Any(x => x.Login == login && x.Password == password);
        if (!userExists)
            return new Result<bool>(new Exception("Пользователь не найден"));

        return true;
    }
}
