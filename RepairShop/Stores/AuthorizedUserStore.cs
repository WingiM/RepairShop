using RepairShop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairShop.Stores;

public class AuthorizedUserStore
{
    public bool IsAuthorized => AuthorizedUser is not null;
    public User? AuthorizedUser { get; private set; }

    public void Authorize(User user)
    {
        if (IsAuthorized)
            return;

        AuthorizedUser = user;
        OnAuthorized?.Invoke(user);
    }

    public void LogOut()
    {
        AuthorizedUser = null;
        OnLogOut?.Invoke();
    }

    public event Action<User>? OnAuthorized;
    public event Action? OnLogOut;
}
