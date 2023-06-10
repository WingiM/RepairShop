using FluentValidation;
using LanguageExt.Common;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace RepairShop.Services.Impl;

public class AuthorizationService : IAuthorizationService
{
    private readonly ApplicationContext _context;
    private readonly IServiceProvider _serviceProdiver;

    public AuthorizationService(ApplicationContext context, 
                                IServiceProvider serviceProdiver)
    {
        _context = context;
        _serviceProdiver = serviceProdiver;
    }

    public Result<User> AuthorizeUser(string login, string password)
    {
        var user = _context.Users.FirstOrDefault(x => x.Login == login && x.Password == password);
        if (user is null)
            return new Result<User>(new Exception("Пользователь не найден"));
        return user;
    }

    public Result<User> RegisterClient(RegisterUserDto user)
    {
        var validationResult = _serviceProdiver.GetRequiredService<IValidator<RegisterUserDto>>().Validate(user);
        if (!validationResult.IsValid)
            return new Result<User>(new ValidationException(validationResult.Errors));

        var userToCreate = new User { Login = user.Login, Password = user.Password, RoleId = (int)Roles.Client };
        _context.Users.Add(userToCreate);

        _context.SaveChanges();
        return userToCreate;
    }
}
