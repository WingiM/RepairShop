using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RepairShop.Attributes;
using RepairShop.ViewModels.Base;

namespace RepairShop.ViewModels;

[Route(Route = Routes.Authorization)]
public partial class AuthorizationViewModel : AuthorizationBaseViewModel
{
    private readonly IAuthorizationService _authorizationService;

    [ObservableProperty] private string _login = string.Empty;

    [ObservableProperty] private string _password = string.Empty;

    public ICommand RegisterCommand { get; private set; }
    public ICommand AuthorizeCommand { get; private set; }

    public AuthorizationViewModel(INavigationService navigationService,
        IAuthorizationService authorizationService,
        AuthorizedUserStore authorizedUserStore) : base(navigationService, authorizedUserStore)
    {
        _authorizationService = authorizationService;
        ViewModelTitle = "Авторизация";
        RegisterCommand = new RelayCommand(() => navigationService.PopAndNavigate(Routes.Registration), () => true);
        AuthorizeCommand = new RelayCommand(() => Authorize(), () => true);
    }

    private void Authorize()
    {
        var result = _authorizationService.AuthorizeUser(Login, Password);
        HandleAuthorizationResult(result);
    }
}