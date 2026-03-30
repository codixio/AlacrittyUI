using System.Globalization;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AlacrittyUI.Services;
using AlacrittyUI.ViewModels;
using AlacrittyUI.Views;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace AlacrittyUI;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; } = null!;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        try
        {
            var services = new ServiceCollection();

            var appSettings = new AppSettingsService();
            appSettings.Load();

            // apply language setting before any UI is created
            var culture = new CultureInfo(appSettings.Settings.Language);
            CultureInfo.CurrentUICulture = culture;
            CultureInfo.CurrentCulture = culture;

            services.AddSingleton(appSettings);

            services.AddSingleton<ConfigDiscoveryService>();
            services.AddSingleton<ConfigReaderService>();
            services.AddSingleton<ConfigWriterService>();
            services.AddSingleton<ThemeService>();
            services.AddSingleton<MainWindowViewModel>();

            Services = services.BuildServiceProvider();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = Services.GetRequiredService<MainWindowViewModel>()
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Failed to initialize application");
            throw;
        }
    }
}
