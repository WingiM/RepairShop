using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RepairShop.Data.Models;
using RepairShop.Navigation;
using RepairShop.Stores;
using System.Linq;

namespace RepairShop.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly NavigationService _navigationService;

    [ObservableProperty]
    private string _userLetter = string.Empty;

    public BaseViewModel CurrentViewModel => _navigationService.CurrentViewModel;

    public RelayCommand GoBackCommand { get; set; }
    public RelayCommand GoToUserPageCommand { get; set; }

    public MainViewModel(NavigationService navigationService, AuthorizedUserStore authorizedUserStore)
    {
        authorizedUserStore.OnAuthorized += AuthorizedUserStore_OnAuthorized;
        authorizedUserStore.OnLogOut += AuthorizedUserStore_OnLogOut;

        _navigationService = navigationService;
        _navigationService.CurrentViewModelChanged += OnCurrentViewModelChanged;

        GoBackCommand = new RelayCommand(() => navigationService.GoBack(), () => navigationService.CanGoBack);
        GoToUserPageCommand = new RelayCommand(() => navigationService.Navigate<AuthorizationViewModel>(), () => authorizedUserStore.IsAuthorized);
        _navigationService.Navigate<AuthorizationViewModel>();
    }

    private void AuthorizedUserStore_OnLogOut()
    {
        GoToUserPageCommand.NotifyCanExecuteChanged();
    }

    private void AuthorizedUserStore_OnAuthorized(User user)
    {
        UserLetter = user.Login[0].ToString();
        GoToUserPageCommand.NotifyCanExecuteChanged();
    }

    private void OnCurrentViewModelChanged()
    {
        OnPropertyChanged(nameof(CurrentViewModel));
        GoBackCommand.NotifyCanExecuteChanged();
    }
}
