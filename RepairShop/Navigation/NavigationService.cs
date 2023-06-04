using Microsoft.Extensions.DependencyInjection;
using RepairShop.ViewModels;
using System;
using System.Collections.Generic;

namespace RepairShop.Navigation;

public class NavigationService
{
    private Dictionary<AppRoutes, Type> routeToType = new Dictionary<AppRoutes, Type>() { { AppRoutes.Authorization, typeof(AuthorizationViewModel)}, { AppRoutes.Register, typeof(RegisterViewModel)} };

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

    private readonly IServiceProvider _serviceProvider;

    public NavigationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void Navigate<TViewModel>() where TViewModel : BaseViewModel
    {
        var viewModelFactory = _serviceProvider.GetRequiredService<ViewModelFactory<TViewModel>>();
        CurrentViewModel = viewModelFactory.GetControl();
    }

    public event Action? CurrentViewModelChanged;

    private void OnCurrentViewModelChanged()
    {
        CurrentViewModelChanged?.Invoke();
    }
}

public enum AppRoutes
{
    Register = 1,
    Authorization = 2
}