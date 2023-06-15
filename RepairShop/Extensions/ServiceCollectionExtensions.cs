using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using RepairShop.Attributes;
using RepairShop.ViewModels.Base;

namespace RepairShop.Extensions;

public static class ServiceCollectionExtensions
{
    public static void RegisterTransientViewModelsFromAssembly(this IServiceCollection services, Assembly assembly)
    {
        var viewModels = assembly
            .GetTypes()
            .Where(x => !x.IsAbstract
                        && x.IsSubclassOf(typeof(BaseViewModel))
                        && x.IsDefined(typeof(ViewModelLifetimeAttribute), true)
                        && GetViewModelLifetimeFromAttribute(x) == ServiceLifetime.Transient);

        foreach (var viewModel in viewModels)
        {
            services.AddTransient(viewModel);
        }
    }
    
    private static ServiceLifetime GetViewModelLifetimeFromAttribute(Type t)
    {
        var attribute = Attribute.GetCustomAttribute(t, typeof(ViewModelLifetimeAttribute))!;
        return ((ViewModelLifetimeAttribute)attribute).Lifetime;
    }
}