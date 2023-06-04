using CommunityToolkit.Mvvm.ComponentModel;
using RepairShop.Navigation;

namespace RepairShop.ViewModels;

public class MainViewModel : ObservableObject
{
    private readonly NavigationService _navigationService;

    public BaseViewModel CurrentViewModel => _navigationService.CurrentViewModel;

    public MainViewModel(NavigationService navigationService)
    {
        _navigationService = navigationService;
        _navigationService.CurrentViewModelChanged += OnCurrentViewModelChanged;

        _navigationService.Navigate<AuthorizationViewModel>();
    }

    private void OnCurrentViewModelChanged()
    {
        OnPropertyChanged(nameof(CurrentViewModel));
    }
}
