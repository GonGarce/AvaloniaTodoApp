using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace AvaloniaTodoApp.Converters;

public class IsEmptyConverter : IValueConverter
{
    public static readonly IsEmptyConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter,
        CultureInfo culture)
    {
        return value switch
        {
            null => false,
            IEnumerable<object> source => !source.Any(),
            _ => new BindingNotification(new InvalidCastException(), BindingErrorType.Error)
        };
    }

    public object ConvertBack(object? value, Type targetType,
        object? parameter, CultureInfo culture)
    {
        return 0;
    }
}