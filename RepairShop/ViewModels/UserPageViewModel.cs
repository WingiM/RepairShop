using RepairShop.Attributes;
using RepairShop.ViewModels.Base;
using System.Diagnostics;

namespace RepairShop.ViewModels;

[Route(Route = Routes.UserPage)]
public partial class UserPageViewModel : BaseViewModel
{
    private readonly AuthorizedUserStore _authorizedUserStore;
    private readonly IAuthorizationService _authorizationService;

    [ObservableProperty]
    private string _login = string.Empty;

    [ObservableProperty]
    private string _oldPassword = string.Empty;

    [ObservableProperty]
    private string _newPassword = string.Empty;

    [ObservableProperty]
    private string _newPasswordRepeat = string.Empty;

    public RelayCommand ChangePasswordCommand { get; set; }
    public RelayCommand LogoutCommand { get; set; }

    public UserPageViewModel(AuthorizedUserStore authorizedUserStore, 
        IAuthorizationService authorizationService)
    {
        ViewModelTitle = "Личный кабинет";
        _authorizationService = authorizationService;
        _authorizedUserStore = authorizedUserStore;

        ChangePasswordCommand = new RelayCommand(() => ChangePassword(), () => true);
        LogoutCommand = new RelayCommand(() => _authorizedUserStore.Logout(), () => true);
    }

    public override NavigationResult OnNavigatedTo(NavigationArgs args)
    {
        Login = _authorizedUserStore.AuthorizedUser!.Login;
        return base.OnNavigatedTo(args);
    }

    private void ChangePassword()
    {
        if (NewPassword != NewPasswordRepeat)
        {
            WriteToSnackBar("Пароли не совпадают");
            return;
        }

        var dto = new ChangeUserPasswordDto { Login = _authorizedUserStore.AuthorizedUser!.Login, NewPassword = NewPassword, OldPassword = OldPassword };
        var result = _authorizationService.ChangePassword(dto);
        result.IfFail(PushErrorToSnackbar);
        result.IfSucc(x => WriteToSnackBar("Пароль успешно изменен"));
        OldPassword = "";
        NewPassword = "";
        NewPasswordRepeat = "";
    }
}
