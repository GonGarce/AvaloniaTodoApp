using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using AvaloniaTodoApp.Global;
using AvaloniaTodoApp.Client;
using AvaloniaTodoApp.ViewModels;
using AvaloniaTodoApp.ViewModels.Collections;
using AvaloniaTodoApp.Views;
using AvaloniaTodoApp.Views;

namespace AvaloniaTodoApp;

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