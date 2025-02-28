using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace AvaloniaTodoApp.Converters;

public class SizeToBorderThicknessConverter : IValueConverter
{
    public static readonly SizeToBorderThicknessConverter Instance = new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo? culture)
    {
        if (value is int size)
        {
            return new Thickness(size / 14f);
        }

        // converter used for the wrong type
        return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int size)
        {
            return new Thickness(size * 14f);
        }

        // converter used for the wrong type
        return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
    }
}