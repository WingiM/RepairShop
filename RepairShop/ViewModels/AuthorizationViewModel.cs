using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RepairShop.ViewModels.Base;

namespace RepairShop.ViewModels;

public partial class AuthorizationViewModel : AuthorizationBaseViewModel
{
    private readonly IAuthorizationService _authorizationService;

    [ObservableProperty] private string _login = string.Empty;

    [ObservableProperty] private string _password = string.Empty;

    public ICommand RegisterCommand { get; private set; }
    public ICommand AuthorizeCommand { get; private set; }

    public AuthorizationViewModel(NavigationService navigationService,
        IAuthorizationService authorizationService,
        AuthorizedUserStore authorizedUserStore) : base(navigationService, authorizedUserStore)
    {
        _authorizationService = authorizationService;
        ViewModelTitle = "Авторизация";
        RegisterCommand = new RelayCommand(() => navigationService.PopAndNavigate<RegisterViewModel>(), () => true);
        AuthorizeCommand = new RelayCommand(() => Authorize(), () => true);
    }

    private void Authorize()
    {
        var result = _authorizationService.AuthorizeUser(Login, Password);
        HandleAuthorizationResult(result);
    }
}