using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RepairShop.Attributes;
using RepairShop.ViewModels.Base;

namespace RepairShop.ViewModels;

[Route(Route = Routes.Registration)]
public partial class RegisterViewModel : AuthorizationBaseViewModel
{
    private readonly IAuthorizationService _authorizationService;

    [ObservableProperty] private string _login = string.Empty;

    [ObservableProperty] private string _password = string.Empty;

    [ObservableProperty] private string _repeatPassword = string.Empty;

    public ICommand RegisterCommand { get; private set; }
    public ICommand AuthorizeCommand { get; private set; }

    public RegisterViewModel(NavigationService navigationService,
        AuthorizedUserStore authorizedUserStore,
        IAuthorizationService authorizationService) : base(navigationService, authorizedUserStore)
    {
        _authorizationService = authorizationService;
        ViewModelTitle = "Регистрация";
        AuthorizeCommand =
            new RelayCommand(() => navigationService.PopAndNavigate(Routes.Authorization), () => true);
        RegisterCommand = new RelayCommand(() => Register(), () => true);
    }

    private void Register()
    {
        var registerDto = new RegisterUserDto { Login = Login, Password = Password, RepeatPassword = RepeatPassword };
        var result = _authorizationService.RegisterClient(registerDto);
        HandleAuthorizationResult(result);
    }
}