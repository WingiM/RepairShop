﻿using System;
using System.Collections.ObjectModel;
using RepairShop.Attributes;
using RepairShop.ViewModels.Base;

namespace RepairShop.Navigation;

public class RouteMap
{
    private readonly IReadOnlyDictionary<string, Type> _routes;

    public RouteMap()
    {
        var viewModels = typeof(BaseViewModel).Assembly
            .GetTypes()
            .Where(x => !x.IsAbstract
                        && x.IsSubclassOf(typeof(BaseViewModel))
                        && x.IsDefined(typeof(RouteAttribute), false))
            .ToDictionary(GetRouteAttributeValueFromClass, x => x);
        _routes = new ReadOnlyDictionary<string, Type>(viewModels);
    }

    public Type? this[string key] => _routes.ContainsKey(key) ? _routes[key] : null;

    private static string GetRouteAttributeValueFromClass(Type t)
    {
        var attribute = Attribute.GetCustomAttribute(t, typeof(RouteAttribute))!;
        return ((RouteAttribute)attribute).Route;
    }
}