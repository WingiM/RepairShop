using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RepairShop.Extensions;
using RepairShop.Services.Impl;
using RepairShop.ViewModels;
using RepairShop.Views;
using System.Globalization;
using System.IO;
using System.Windows;
using RepairShop.ViewModels.Base;

namespace RepairShop;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    private void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddValidatorsFromAssembly(typeof(App).Assembly);
        services.AddScoped<IAuthorizationService, AuthorizationService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRequestService, RequestService>();
        services.AddDbContext<ApplicationContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Database"),
                npgsqlOptions => npgsqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        services.AddSingleton(configuration);

        ConfigurePresentation(services);
    }

    private static void ConfigurePresentation(IServiceCollection services)
    {
        services.AddNavigation<BaseViewModel>();
        services.AddSingleton<AuthorizedUserStore>();

        services.AddViewModelFactory<AuthorizationViewModel>();
        services.AddViewModelFactory<RegisterViewModel>();
        services.AddViewModelFactory<ClientPageViewModel>();
        services.AddViewModelFactory<RequestPageViewModel>();
        services.AddSingleton<MainViewModel>();

        services.AddSingleton(typeof(MainWindow));
    }

    protected override void OnStartup(StartupEventArgs args)
    {
        var cultureInfo = new CultureInfo("ru-RU");
        CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        var configuration = builder.Build();

        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection, configuration);

        var serviceProvider = serviceCollection.BuildServiceProvider();

        var context = serviceProvider.GetRequiredService<ApplicationContext>();
        // warmup query
        context.Database.ExecuteSql($"SELECT 1 FROM user");
        var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }
}