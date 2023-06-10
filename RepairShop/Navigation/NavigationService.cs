using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
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

    public void Navigate<TViewModel>() where TViewModel : BaseViewModel
    {
        var viewModelFactory = _serviceProvider.GetRequiredService<ViewModelFactory<TViewModel>>();
        var viewModel = viewModelFactory.GetControl();
        _routeHistory.Push(viewModel);
        CurrentViewModel = viewModel;
    }

    public void PopAndNavigate<TViewModel>() where TViewModel : BaseViewModel
    {
        _routeHistory.Pop();
        var viewModelFactory = _serviceProvider.GetRequiredService<ViewModelFactory<TViewModel>>();
        var viewModel = viewModelFactory.GetControl();
        _routeHistory.Push(viewModel);
        CurrentViewModel = viewModel;
    }

    public void ClearAndNavigate<TViewModel>() where TViewModel : BaseViewModel
    {
        while (_routeHistory.Count > 0)
        {
            _routeHistory.Pop();
        }
        var viewModelFactory = _serviceProvider.GetRequiredService<ViewModelFactory<TViewModel>>();
        var viewModel = viewModelFactory.GetControl();
        _routeHistory.Push(viewModel);
        CurrentViewModel = viewModel;
    }

    public void GoBack()
    {
        if (!CanGoBack) return;
        _routeHistory.Pop();
        CurrentViewModel = _routeHistory.Peek();
    }

    public event Action? CurrentViewModelChanged;

    private void OnCurrentViewModelChanged()
    {
        CurrentViewModelChanged?.Invoke();
    }
}