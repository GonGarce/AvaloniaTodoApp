using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace AvaloniaTodoApp.Converters;

public class SizeToFontSizeConverter : IValueConverter
{
    public static readonly SizeToFontSizeConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int size)
        {
            return size / 2.5;
        }

        // converter used for the wrong type
        return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int size)
        {
            return size * 2.5;
        }

        // converter used for the wrong type
        return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
    }
}