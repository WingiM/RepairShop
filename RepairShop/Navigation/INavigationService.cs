using RepairShop.ViewModels.Base;
using System;

namespace RepairShop.Navigation;

public interface INavigationService
{
    public BaseViewModel CurrentViewModel { get; }
    public bool CanGoBack { get; }

    public bool Navigate(string path, DynamicDictionary? parameters = null);
    public bool PopAndNavigate(string path, DynamicDictionary? parameters = null);
    public bool ClearAndNavigate(string path, DynamicDictionary? parameters = null);
    public void GoBack();

    public event Action<NavigationArgs>? OnNavigated;
}