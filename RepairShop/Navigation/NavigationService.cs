using Microsoft.Extensions.DependencyInjection;
using System;
using RepairShop.ViewModels.Base;

namespace RepairShop.Navigation;

public class NavigationService
{
    private readonly Stack<BaseViewModel> _routeHistory;

    private BaseViewModel _currentViewModel = null!;
    public BaseViewModel CurrentViewModel
    {
        get => _currentViewModel;
        private set
        {
            _currentViewModel = value;
            OnCurrentViewModelChanged();
        }
    }

    public bool CanGoBack => _routeHistory.Length() != 1;

    private readonly IServiceProvider _serviceProvider;

    public NavigationService(IServiceProvider serviceProvider)
    {
        _routeHistory = new Stack<BaseViewModel>();
        _serviceProvider = serviceProvider;
    }

    public void Navigate<TViewModel>(NavigationArgs? args = null) where TViewModel : BaseViewModel
    {
        var viewModelFactory = _serviceProvider.GetRequiredService<ViewModelFactory<TViewModel>>();
        var viewModel = viewModelFactory.GetViewModel();
        _routeHistory.Push(viewModel);
        viewModel.OnNavigatedTo(args);
        CurrentViewModel = viewModel;
    }

    public void PopAndNavigate<TViewModel>(NavigationArgs? args = null) where TViewModel : BaseViewModel
    {
        _routeHistory.Pop();
        Navigate<TViewModel>(args);
    }

    public void ClearAndNavigate<TViewModel>(NavigationArgs? args = null) where TViewModel : BaseViewModel
    {
        while (_routeHistory.Count > 0)
        {
            _routeHistory.Pop();
        }
        Navigate<TViewModel>(args);
    }

    public void GoBack()
    {
        if (!CanGoBack) return;
        _routeHistory.Pop();
        CurrentViewModel = _routeHistory.Peek();
        CurrentViewModel.OnNavigatedTo();
    }

    public event Action? CurrentViewModelChanged;

    private void OnCurrentViewModelChanged()
    {
        CurrentViewModelChanged?.Invoke();
    }
}