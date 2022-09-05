using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectIndiaCharlie.Desktop.ViewModels;
using ProjectIndiaCharlie.Desktop.ViewModels.Service;
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

                services.AddSingleton<MainViewModel>();
                services.AddSingleton<IndexViewModel>();
                services.AddSingleton<HomeViewModel>();
                services.AddSingleton<LoginViewModel>();
                services.AddSingleton<RegisterStudentViewModel>();
            })
            .Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await AppHost!.StartAsync();

        NavigationService.MainNavigate(new LoginViewModel());

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
