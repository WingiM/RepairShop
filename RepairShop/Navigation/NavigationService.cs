using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using RepairShop.ViewModels;
using System;
using System.Collections.Generic;

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
        _routeHistory.Push(CurrentViewModel);
        var viewModelFactory = _serviceProvider.GetRequiredService<ViewModelFactory<TViewModel>>();
        var viewModel = viewModelFactory.GetControl();
        CurrentViewModel = viewModel;
    }

    public void PopAndNavigate<TViewModel>() where TViewModel : BaseViewModel
    {
        _routeHistory.Pop();
        _routeHistory.Push(CurrentViewModel);
        var viewModelFactory = _serviceProvider.GetRequiredService<ViewModelFactory<TViewModel>>();
        var viewModel = viewModelFactory.GetControl();
        CurrentViewModel = viewModel;
    }

    public void ClearAndNavigate<TViewModel>() where TViewModel : BaseViewModel
    {
        while (_routeHistory.Count > 0)
        {
            _routeHistory.Pop();
        }
        _routeHistory.Push(CurrentViewModel);
        var viewModelFactory = _serviceProvider.GetRequiredService<ViewModelFactory<TViewModel>>();
        var viewModel = viewModelFactory.GetControl();
        CurrentViewModel = viewModel;
    }

    public void GoBack()
    {
        if (!CanGoBack) return;
        var viewModel = _routeHistory.Pop();
        CurrentViewModel = viewModel;
    }

    public event Action? CurrentViewModelChanged;

    private void OnCurrentViewModelChanged()
    {
        CurrentViewModelChanged?.Invoke();
    }
}