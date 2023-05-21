using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;

namespace BeatForgeClient.Utility;

public class EnumDescriptionConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not Enum)
            return null;

        return value.GetType()
            .GetField(value.ToString()!)!
            .GetCustomAttributes(false)
            .OfType<DescriptionAttribute>()
            .First().Description;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}