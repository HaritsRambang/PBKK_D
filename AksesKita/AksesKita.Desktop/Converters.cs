using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using AksesKita.Desktop.Models;

namespace AksesKita.Desktop;

/// <summary>True when the bound page key equals the ConverterParameter. Used to highlight the active sidebar item.</summary>
public class PageToBoolConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => string.Equals(value as string, parameter as string, StringComparison.Ordinal);

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => Binding.DoNothing;
}

/// <summary>Maps a severity level to an accent brush for chips and status dots.</summary>
public class SeverityToBrushConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var color = value is SeverityLevel s
            ? s switch
            {
                SeverityLevel.Kritis => "#DC2626",
                SeverityLevel.Sedang => "#B45309",
                _ => "#16A34A"
            }
            : "#64748B";
        return new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => Binding.DoNothing;
}

/// <summary>Light background tint that pairs with <see cref="SeverityToBrushConverter"/>.</summary>
public class SeverityToTintConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var color = value is SeverityLevel s
            ? s switch
            {
                SeverityLevel.Kritis => "#FEF2F2",
                SeverityLevel.Sedang => "#FFFBEB",
                _ => "#F0FDF4"
            }
            : "#F1F5F9";
        return new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => Binding.DoNothing;
}

/// <summary>Foreground colour for a report status chip.</summary>
public class StatusToBrushConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => new SolidColorBrush((Color)ColorConverter.ConvertFromString(
            value is ReportStatus s ? Formatters.StatusColor(s) : "#475569"));

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => Binding.DoNothing;
}

/// <summary>Soft background for a report status chip.</summary>
public class StatusToTintConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var color = value is ReportStatus s
            ? s switch
            {
                ReportStatus.Baru => "#F1F5F9",
                ReportStatus.Terverifikasi => "#E0F2FE",
                ReportStatus.DalamPerbaikan => "#CFFAFE",
                ReportStatus.Selesai => "#DCFCE7",
                _ => "#FFE4E6"
            }
            : "#F1F5F9";
        return new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => Binding.DoNothing;
}

/// <summary>Turns an enum value into readable text ("DalamPerbaikan" → "Dalam Perbaikan").</summary>
public class EnumLabelConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => Formatters.Label(value);

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => Binding.DoNothing;
}

/// <summary>Visible when the bound value is false (the built-in converter only does the opposite).</summary>
public class InverseBoolToVisibilityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is true ? Visibility.Collapsed : Visibility.Visible;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => Binding.DoNothing;
}

/// <summary>Visible when the bound string is an existing file; pass "invert" to flip it.</summary>
public class FileExistsToVisibilityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var exists = value is string path && !string.IsNullOrWhiteSpace(path) && File.Exists(path);
        if (string.Equals(parameter as string, "invert", StringComparison.OrdinalIgnoreCase)) exists = !exists;
        return exists ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => Binding.DoNothing;
}
