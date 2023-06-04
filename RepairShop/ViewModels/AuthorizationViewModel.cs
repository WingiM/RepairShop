using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RepairShop.Navigation;
using System.Windows.Input;

namespace RepairShop.ViewModels;

public partial class AuthorizationViewModel : BaseViewModel
{
    [ObservableProperty]
    private string _login = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    public ICommand RegisterCommand { get; private set; }

    public AuthorizationViewModel(NavigationService navigationService)
    {
        ViewModelTitle = "Авторизация";
        RegisterCommand = new RelayCommand(() => navigationService.PopAndNavigate<RegisterViewModel>(), () => true);
    }
}
