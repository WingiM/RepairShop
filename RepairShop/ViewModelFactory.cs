using System;
using RepairShop.ViewModels.Base;

namespace RepairShop;

public class ViewModelFactory<TViewModel> where TViewModel : BaseViewModel
{
    private readonly Func<TViewModel> _factoryMethod;

    public ViewModelFactory(Func<TViewModel> factoryMethod)
    {
        _factoryMethod = factoryMethod;
    }

    public TViewModel GetViewModel() => _factoryMethod();
}
