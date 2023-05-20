using System;
using System.Globalization;
using AutoMapper;
using Avalonia.Data.Converters;

namespace BeatForgeClient.Utility;

public class PlaybackConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int playback)
        {
            return playback * 20.0;
        }

        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}