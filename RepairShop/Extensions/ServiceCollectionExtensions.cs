using Microsoft.Extensions.DependencyInjection;
using RepairShop.ViewModels;

namespace RepairShop.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddViewModelFactory<TViewModel>(this IServiceCollection services)
        where TViewModel : BaseViewModel
    {
        services.AddTransient<TViewModel>();
        services.AddSingleton(x => new ViewModelFactory<TViewModel>(() => x.GetRequiredService<TViewModel>()));
    }
}
