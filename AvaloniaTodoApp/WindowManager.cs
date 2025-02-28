using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using AvaloniaTodoApp.Global;
using AvaloniaTodoApp.ViewModels;
using AvaloniaTodoApp.Views;
using AvaloniaTodoApp.Views;

namespace AvaloniaTodoApp;

public class WindowManager
{
    public static void ChangeWindow(Func<Window> newWindowFunc)
    {
        if (Application.Current!.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
        {
            var oldWindow = desktopLifetime.MainWindow;

            var newWindow = newWindowFunc();
            desktopLifetime.MainWindow = newWindow;

            // Show main window to avoid framework shutdown when closing splash screen
            newWindow.Show();

            // Finally, close the splash screen
            oldWindow?.Close();
        }
    }

    public static void ShowMainWindow()
    {
        if (AppState.Test)
        {
            ChangeWindow(() => new TestWindow { DataContext = new TestWindowViewModel() });
            return;
        }
        
        ChangeWindow(() => new MainWindow { DataContext = new MainWindowViewModel() });
    }
}