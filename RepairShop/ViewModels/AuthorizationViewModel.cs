using CommunityToolkit.Mvvm.Input;
using RepairShop.Navigation;
using System.Windows.Input;

namespace RepairShop.ViewModels;

public class AuthorizationViewModel : BaseViewModel
{
    public AuthorizationViewModel(NavigationService navigationService)
    {
        ViewModelTitle = "Авторизация";
        RegisterCommand = new RelayCommand(() => navigationService.Navigate<RegisterViewModel>(), () => true);
    }

    public ICommand RegisterCommand { get; private set; }
}
