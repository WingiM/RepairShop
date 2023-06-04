using CommunityToolkit.Mvvm.ComponentModel;

namespace RepairShop.ViewModels;

public abstract partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    private string _viewModelTitle = "Ремонтная мастерская";
}
