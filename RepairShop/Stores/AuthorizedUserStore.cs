using System;

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

    public void Logout()
    {
        AuthorizedUser = null;
        OnLogout?.Invoke();
    }

    public event Action<User>? OnAuthorized;
    public event Action? OnLogout;
}
