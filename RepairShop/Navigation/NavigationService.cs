using System;
using System.Windows.Navigation;
using RepairShop.ViewModels.Base;

namespace RepairShop.Navigation;

public class NavigationService : INavigationService
{
    private readonly Stack<KeyValuePair<string, BaseViewModel>> _routeHistory;

    public BaseViewModel CurrentViewModel { get; private set; } = null!;

    public bool CanGoBack => _routeHistory.Length() != 1;

    private readonly IServiceProvider _serviceProvider;
    private readonly RouteMap _routeMap;

    public NavigationService(IServiceProvider serviceProvider, RouteMap routeMap)
    {
        _routeHistory = new Stack<KeyValuePair<string, BaseViewModel>>();
        _serviceProvider = serviceProvider;
        _routeMap = routeMap;
    }

    public bool Navigate(string path, DynamicDictionary? parameters = null)
    {
        if (!PathExists(path)) return false;

        var viewModelType = _routeMap[path]!;
        if (_serviceProvider.GetService(viewModelType) is not BaseViewModel viewModel)
            return false;

        var args = new NavigationArgs { Destination = path, Parameters = parameters ?? new DynamicDictionary() };
        _routeHistory.Push(new KeyValuePair<string, BaseViewModel>(path, viewModel));
        NavigateInternal(viewModel, args);
        return true;
    }

    public bool PopAndNavigate(string path, DynamicDictionary? parameters = null)
    {
        if (!PathExists(path)) return false;
        _routeHistory.Pop();
        return Navigate(path, parameters);
    }

    public bool ClearAndNavigate(string path, DynamicDictionary? parameters = null)
    {
        if (!PathExists(path)) return false;
        _routeHistory.Clear();
        return Navigate(path, parameters);
    }

    public void GoBack()
    {
        if (!CanGoBack) return;
        _routeHistory.Pop();
        var viewModel = _routeHistory.Peek();
        var args = new NavigationArgs { Destination = viewModel.Key, NavigationMode = NavigationMode.Back };
        NavigateInternal(viewModel.Value, args);
    }

    private void NavigateInternal(BaseViewModel viewModel, NavigationArgs args)
    {
        viewModel.OnNavigatedTo(args);
        CurrentViewModel = viewModel;
        OnNavigated?.Invoke(args);
    }

    private bool PathExists(string path) => _routeMap[path] is not null;

    public event Action<NavigationArgs>? OnNavigated;
}