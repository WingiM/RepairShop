using RepairShop.ViewModels.Base;
using System;

namespace RepairShop.Navigation;

public interface INavigationService
{
    public BaseViewModel CurrentViewModel { get; }
    public bool CanGoBack { get; }

    public NavigationResult Navigate(string path, DynamicDictionary? parameters = null);
    public NavigationResult PopAndNavigate(string path, DynamicDictionary? parameters = null);
    public NavigationResult ClearAndNavigate(string path, DynamicDictionary? parameters = null);
    public void GoBack();

    public event Action<NavigationResult>? OnNavigated;
}