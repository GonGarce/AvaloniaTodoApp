using Avalonia;
using Avalonia.Styling;

namespace AvaloniaTodoApp.Global;

public static class ThemeManager
{
    public static void ToggleTheme()
    {
        if (Application.Current!.ActualThemeVariant == ThemeVariant.Light)
        {
            Application.Current.RequestedThemeVariant = ThemeVariant.Dark;
        }
        else
        {
            Application.Current.RequestedThemeVariant = ThemeVariant.Light;
        }
    }
}