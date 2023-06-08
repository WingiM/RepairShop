using CommunityToolkit.Mvvm.ComponentModel;
using MaterialDesignThemes.Wpf;

namespace RepairShop.ViewModels;

public abstract partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    private string _viewModelTitle = "Ремонтная мастерская";

    [ObservableProperty]
    private SnackbarMessageQueue _snackbarMessageQueue;

    protected BaseViewModel()
    {
        SnackbarMessageQueue = new SnackbarMessageQueue();
    }
}
