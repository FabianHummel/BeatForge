using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;

namespace BeatForgeClient.Utility;

public class EnumConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not Enum)
            return null;

        return Enum.GetValues(value.GetType())
            .Cast<Enum>()
            .Select(e => e.GetType()
                .GetField(e.ToString())!
                .GetCustomAttributes(false)
                .OfType<DescriptionAttribute>()
                .First().Description);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}