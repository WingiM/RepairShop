using LanguageExt.Common;
using RepairShop.Data;
using RepairShop.Data.Models;
using System;
using System.Linq;

namespace RepairShop.Services.Impl;

public class AuthorizationService : IAuthorizationService
{
    private readonly ApplicationContext _context;

    public AuthorizationService(ApplicationContext context)
    {
        _context = context;
    }

    public Result<User> AuthorizeUser(string login, string password)
    {
        var user = _context.Users.FirstOrDefault(x => x.Login == login && x.Password == password);
        if (user is null)
            return new Result<User>(new Exception("Пользователь не найден"));

        return user;
    }
}
