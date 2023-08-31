using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace BeatForgeClient.Utility;

public class PlayingConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool playing && targetType == typeof(double))
        {
            return playing ? 0.3 : 1.0;
        }

        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}