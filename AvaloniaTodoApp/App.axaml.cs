using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using AvaloniaTodoApp.App;
using AvaloniaTodoAPp.Client;
using AvaloniaTodoAPp.ViewModels;
using AvaloniaTodoApp.ViewModels.Collections;
using AvaloniaTodoApp.Views;
using AvaloniaTodoAPp.Views;

namespace AvaloniaTodoAPp;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Line below is needed to remove Avalonia data validation.
            // Without this line you will get duplicate validations from both Avalonia and CT
            BindingPlugins.DataValidators.RemoveAt(0);

            // Show splash screen window
            desktop.MainWindow = new SplashWindow();

            // Some heavy init tasks
            AppState.Instance.Initialize()
                .ContinueWith(_ => { Dispatcher.UIThread.Post(LoginPage.LoginOrLoad); });
        }

        base.OnFrameworkInitializationCompleted();
    }
}