using LanguageExt.Common;

namespace RepairShop.ViewModels.Base;

public abstract partial class AuthorizationBaseViewModel : BaseViewModel
{
    private readonly NavigationService _navigationService;
    private readonly AuthorizedUserStore _authorizedUserStore;

    protected AuthorizationBaseViewModel(NavigationService navigationService,
        AuthorizedUserStore authorizedUserStore)
    {
        _navigationService = navigationService;
        _authorizedUserStore = authorizedUserStore;
    }

    protected void HandleAuthorizationResult(Result<User> authorizationResult)
    {
        authorizationResult.IfSucc(HandleAuthorizationSuccess);
        authorizationResult.IfFail(PushErrorToSnackbar);
    }

    private void HandleAuthorizationSuccess(User user)
    {
        _authorizedUserStore.Authorize(user);
        switch (user.RoleId)
        {
            case (int)Roles.Client:
                _navigationService.PopAndNavigate<ClientPageViewModel>();
                break;
        }
    }
}