using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using RepairShop.Navigation;
using RepairShop.Services;
using RepairShop.Stores;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace RepairShop.ViewModels;

public partial class AuthorizationViewModel : BaseViewModel
{
    private readonly IAuthorizationService _authorizationService;
    private readonly NavigationService _navigationService;
    private readonly AuthorizedUserStore _authorizedUserStore;

    [ObservableProperty]
    private string _login = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    public ICommand RegisterCommand { get; private set; }
    public ICommand AuthorizeCommand { get; private set; }

    public AuthorizationViewModel(NavigationService navigationService, IAuthorizationService authorizationService, AuthorizedUserStore authorizedUserStore)
    {
        _authorizationService = authorizationService;
        _navigationService = navigationService;
        ViewModelTitle = "Авторизация";
        RegisterCommand = new RelayCommand(() => navigationService.PopAndNavigate<RegisterViewModel>(), () => true);
        AuthorizeCommand = new RelayCommand(() => Authorize(), () => true);
        _authorizedUserStore = authorizedUserStore;
    }

    private void Authorize()
    {
        var result = _authorizationService.AuthorizeUser(Login, Password);
        if (result.IsSuccess)
        {
            result.IfSucc(x =>
            {
                _authorizedUserStore.Authorize(x);
            });
            SnackbarMessageQueue.Enqueue("Успешно");
            _navigationService.PopAndNavigate<RegisterViewModel>();
            return;
        }

        result.IfFail(x =>
        {
            var message = x is ValidationException ve ? string.Join('\n', ve.Errors.Select(x => x.ErrorMessage)) : x.Message;
            SnackbarMessageQueue.Enqueue(message);
        });

    }
}
