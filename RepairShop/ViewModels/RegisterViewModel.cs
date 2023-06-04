using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RepairShop.Services;
using System.Windows.Input;
using System.Windows;
using RepairShop.Navigation;
using FluentValidation;
using System.Linq;
using RepairShop.Data.DTO;

namespace RepairShop.ViewModels;

public partial class RegisterViewModel : BaseViewModel
{
    private readonly IAuthorizationService _authorizationService;
    private readonly NavigationService _navigationService;

    [ObservableProperty]
    private string _login = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private string _repeatPassword = string.Empty;

    public ICommand RegisterCommand { get; private set; }
    public ICommand AuthorizeCommand { get; private set; }

    public RegisterViewModel(NavigationService navigationService, IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
        _navigationService = navigationService;
        ViewModelTitle = "Регистрация";
        AuthorizeCommand = new RelayCommand(() => navigationService.PopAndNavigate<AuthorizationViewModel>(), () => true);
        RegisterCommand = new RelayCommand(() => Register(), () => true);
    }

    private void Register()
    {
        var registerDto = new RegisterUserDto { Login = Login, Password = Password, RepeatPassword = RepeatPassword };
        var result = _authorizationService.RegisterClient(registerDto);
        result.IfFail(x =>
        {
            var message = x is ValidationException ve ? string.Join('\n', ve.Errors.Select(x => x.ErrorMessage)) : x.Message;
            MessageBox.Show(message);
        });

        result.IfSucc(x =>
        {
            MessageBox.Show("Успешно");
            _navigationService.PopAndNavigate<AuthorizationViewModel>();
        });
    }
}
