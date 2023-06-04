﻿using RepairShop.ViewModels;
using System;

namespace RepairShop;

public class ViewModelFactory<TViewModel> where TViewModel : BaseViewModel
{
    private readonly Func<TViewModel> _factoryMethod;

    public ViewModelFactory(Func<TViewModel> factoryMethod)
    {
        _factoryMethod = factoryMethod;
    }

    public TViewModel GetControl() => _factoryMethod();
}
