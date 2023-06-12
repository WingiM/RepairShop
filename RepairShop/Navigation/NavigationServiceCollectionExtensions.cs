using Microsoft.Extensions.DependencyInjection;

namespace RepairShop.Navigation;

public static class NavigationServiceCollectionExtensions
{
    public static IServiceCollection AddNavigation<T>(this IServiceCollection services) where T : class, INavigatable
    {
        services.AddSingleton<RouteMap<T>>();
        services.AddSingleton<INavigationService<T>, NavigationService<T>>();
        return services;
    }
}