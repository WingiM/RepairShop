using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RepairShop.ViewModels.Base;

namespace RepairShop.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly NavigationService _navigationService;

    [ObservableProperty]
    private string _userLetter = string.Empty;

    public BaseViewModel CurrentViewModel => _navigationService.CurrentViewModel;

    public RelayCommand GoBackCommand { get; set; }
    public RelayCommand GoToUserPageCommand { get; set; }

    public MainViewModel(NavigationService navigationService, 
        AuthorizedUserStore authorizedUserStore, 
        IUserService userService)
    {
        authorizedUserStore.OnAuthorized += AuthorizedUserStore_OnAuthorized;
        authorizedUserStore.OnLogout += AuthorizedUserStore_OnLogout;

        _navigationService = navigationService;
        _navigationService.OnNavigated += OnNavigated;

        GoBackCommand = new RelayCommand(() => navigationService.GoBack(), () => navigationService.CanGoBack);
        GoToUserPageCommand = new RelayCommand(() => navigationService.Navigate(Routes.Authorization), () => authorizedUserStore.IsAuthorized);

        var authorizedUserId = Properties.Settings.Default.LoggedUserId;
        var user = userService.GetUser(authorizedUserId);
        if (user is not null)
        {
            authorizedUserStore.Authorize(user);
            if (user.RoleId == (int)Roles.Client)
                _navigationService.Navigate(Routes.ClientPage);
            return;
        }
        _navigationService.Navigate(Routes.Authorization);
    }

    private void AuthorizedUserStore_OnLogout()
    {
        Properties.Settings.Default.LoggedUserId = 0;
        Properties.Settings.Default.Save();
        GoToUserPageCommand.NotifyCanExecuteChanged();
    }

    private void AuthorizedUserStore_OnAuthorized(User user)
    {
        UserLetter = user.Login[0].ToString();
        Properties.Settings.Default.LoggedUserId = user.Id;
        Properties.Settings.Default.Save();
        GoToUserPageCommand.NotifyCanExecuteChanged();
    }

    private void OnNavigated(NavigationArgs args)
    {
        OnPropertyChanged(nameof(CurrentViewModel));
        GoBackCommand.NotifyCanExecuteChanged();
    }
}
