using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RepairShop.ViewModels.Base;

namespace RepairShop.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly INavigationService<BaseViewModel> _navigationService;
    private readonly AuthorizedUserStore _authorizedUserStore;

    [ObservableProperty] private string _userLetter = string.Empty;

    public BaseViewModel CurrentViewModel => _navigationService.CurrentNavigatedItem;

    public RelayCommand GoBackCommand { get; set; }
    public RelayCommand GoToUserPageCommand { get; set; }
    public RelayCommand GoHomeCommand { get; set; }

    public MainViewModel(INavigationService<BaseViewModel> navigationService,
        AuthorizedUserStore authorizedUserStore,
        IUserService userService)
    {
        authorizedUserStore.OnAuthorized += AuthorizedUserStore_OnAuthorized;
        authorizedUserStore.OnLogout += AuthorizedUserStore_OnLogout;

        _navigationService = navigationService;
        _authorizedUserStore = authorizedUserStore;
        _navigationService.OnNavigated += OnNavigated;

        GoBackCommand = new RelayCommand(() => navigationService.GoBack(), () => navigationService.CanGoBack);
        GoToUserPageCommand = new RelayCommand(() => navigationService.Navigate(Routes.UserPage),
            () => authorizedUserStore.IsAuthorized);
        GoHomeCommand = new RelayCommand(() => NavigateHome(), () => true);

        var authorizedUserId = Properties.Settings.Default.LoggedUserId;
        var user = userService.GetUser(authorizedUserId);
        if (user is not null)
        {
            authorizedUserStore.Authorize(user);
            NavigateHome();
            return;
        }

        _navigationService.Navigate(Routes.Authorization);
    }

    private void NavigateHome()
    {
        if (!_authorizedUserStore.IsAuthorized)
        {
            _navigationService.ClearAndNavigate(Routes.Authorization);
            return;
        }
        if (_authorizedUserStore.AuthorizedUser!.RoleId == (int)Roles.Client)
            _navigationService.ClearAndNavigate(Routes.ClientPage);
        if (_authorizedUserStore.AuthorizedUser!.RoleId == (int)Roles.Master)
            _navigationService.ClearAndNavigate(Routes.MasterPage);
    }

    private void AuthorizedUserStore_OnLogout()
    {
        Properties.Settings.Default.LoggedUserId = 0;
        Properties.Settings.Default.Save();
        GoToUserPageCommand.NotifyCanExecuteChanged();
        _navigationService.ClearAndNavigate(Routes.Authorization);
    }

    private void AuthorizedUserStore_OnAuthorized(User user)
    {
        UserLetter = user.Login[0].ToString().ToUpper();
        Properties.Settings.Default.LoggedUserId = user.Id;
        Properties.Settings.Default.Save();
        GoToUserPageCommand.NotifyCanExecuteChanged();
    }

    private void OnNavigated(NavigationResult result)
    {
        if (!result.IsSuccess)
        {
            CurrentViewModel.WriteToSnackBar(result.Message ?? "Произошла ошибка при навигации");
            return;
        }

        OnPropertyChanged(nameof(CurrentViewModel));
        GoBackCommand.NotifyCanExecuteChanged();
    }
}