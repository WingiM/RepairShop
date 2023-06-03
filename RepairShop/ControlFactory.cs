using System;
using System.Windows;

namespace RepairShop;

public class ControlFactory<TControl> where TControl : UIElement
{
    private readonly Func<TControl> _factoryMethod;

    public ControlFactory(Func<TControl> factoryMethod)
    {
        _factoryMethod = factoryMethod;
    }

    public TControl GetControl() => _factoryMethod();
}
