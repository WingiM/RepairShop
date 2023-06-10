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
        if (authorizationResult.IsSuccess)
        {
            authorizationResult.IfSucc(_authorizedUserStore.Authorize);

            if (_authorizedUserStore.AuthorizedUser!.RoleId == (int)Roles.Client)
                _navigationService.PopAndNavigate<ClientPageViewModel>();
            return;
        }

        authorizationResult.IfFail(PushErrorToSnackbar);
    }
}