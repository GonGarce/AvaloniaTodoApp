using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace AvaloniaTodoApp.Converters;

public class CollectionCountConverter : IValueConverter
{
    public static readonly CollectionCountConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter,
        CultureInfo culture)
    {

        if (value is IEnumerable<object> source)
        {
            return source.Count();
        }

        // converter used for the wrong type
        return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
    }

    public object ConvertBack(object? value, Type targetType,
        object? parameter, CultureInfo culture)
    {
        return 0;
    }
}