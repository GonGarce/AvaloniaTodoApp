using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace AvaloniaTodoApp.Converters;

public class GreaterCountConverter : IValueConverter
{
    public static readonly GreaterCountConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter,
        CultureInfo culture)
    {
        var p = string.IsNullOrEmpty(parameter as string) ? "0" : (string) parameter;
        var count = int.Parse(p);
        return value switch
        {
            null => false,
            IEnumerable<object> source => source.Count() > count,
            _ => new BindingNotification(new InvalidCastException(), BindingErrorType.Error)
        };
    }

    public object ConvertBack(object? value, Type targetType,
        object? parameter, CultureInfo culture)
    {
        return 0;
    }
}