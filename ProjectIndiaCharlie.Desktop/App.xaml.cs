using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectIndiaCharlie.Desktop.ViewModel;
using ProjectIndiaCharlie.Desktop.ViewModel.Service;
using ProjectIndiaCharlie.Desktop.ViewModel.Store;
using System.Windows;

namespace ProjectIndiaCharlie.Desktop;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static IHost? AppHost { get; private set; }

    public App()
    {
        AppHost = Host.CreateDefaultBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<MainWindow>();

                services.AddSingleton<IndexViewModel>();
                services.AddSingleton<HomeViewModel>();
            })
            .Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await AppHost!.StartAsync();

        NavigationService.MainNavigate(new IndexViewModel());
        NavigationService.IndexNavigate(new HomeViewModel());

        var startupWindow = AppHost.Services.GetRequiredService<MainWindow>();
        startupWindow.DataContext = new MainViewModel();
        startupWindow.Show();

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await AppHost!.StopAsync();

        base.OnExit(e);
    }
}
