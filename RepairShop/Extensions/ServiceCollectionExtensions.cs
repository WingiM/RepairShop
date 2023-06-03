using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace RepairShop.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddControlFactory<TControl>(this IServiceCollection services)
        where TControl : UIElement
    {
        services.AddTransient<TControl>();
        services.AddSingleton(x => new ControlFactory<TControl>(() => x.GetRequiredService<TControl>()));
    }
}
