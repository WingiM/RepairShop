using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RepairShop.Navigation;

namespace RepairShop.ViewModels;

public class MainViewModel : ObservableObject
{
    private readonly NavigationService _navigationService;

    public BaseViewModel CurrentViewModel => _navigationService.CurrentViewModel;

    public RelayCommand GoBackCommand { get; set; }

    public MainViewModel(NavigationService navigationService)
    {
        _navigationService = navigationService;
        _navigationService.CurrentViewModelChanged += OnCurrentViewModelChanged;

        GoBackCommand = new RelayCommand(() => navigationService.GoBack(), () => navigationService.CanGoBack);
        _navigationService.Navigate<AuthorizationViewModel>();
    }

    private void OnCurrentViewModelChanged()
    {
        OnPropertyChanged(nameof(CurrentViewModel));
        GoBackCommand.NotifyCanExecuteChanged();
    }
}
