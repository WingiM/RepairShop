using System;
using System.Collections.ObjectModel;
using RepairShop.Attributes;

namespace RepairShop.Navigation;

public class RouteMap<T> where T : class, INavigatable
{
    private readonly IReadOnlyDictionary<string, Type> _routes;

    public RouteMap()
    {
        var viewModels = typeof(T).Assembly
            .GetTypes()
            .Where(x => !x.IsAbstract
                        && x.IsSubclassOf(typeof(T))
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