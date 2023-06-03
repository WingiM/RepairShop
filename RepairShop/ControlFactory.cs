using System;
using System.Windows;

namespace RepairShop;

public class ControlFactory<TControl> where TControl : UIElement
{
    private readonly Func<TControl> factoryMethod;

    public ControlFactory(Func<TControl> factoryMethod)
    {
        this.factoryMethod = factoryMethod;
    }

    public TControl GetControl() => factoryMethod();
}
