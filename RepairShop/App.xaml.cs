using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.IO;
using System.Windows;

namespace RepairShop;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private IServiceProvider _serviceProvider = null!;
    public IConfiguration Configuration { get; private set; } = null!;

    private void ConfigureServices(IServiceCollection services)
    {
        ConfigurePresentation(services);
    }
    
    private void ConfigurePresentation(IServiceCollection services)
    {
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

        Configuration = builder.Build();

        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);

        _serviceProvider = serviceCollection.BuildServiceProvider();

        var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }
}
