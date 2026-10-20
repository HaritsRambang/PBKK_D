using System;
using System.IO;
using System.Windows;

namespace AksesKita.Desktop.Services;

/// <summary>Switches between the light and dark colour dictionaries and remembers the choice.</summary>
public static class ThemeService
{
    private static readonly string PrefPath =
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "theme.txt");

    public static bool IsDark { get; private set; }

    public static void LoadSaved()
    {
        var dark = false;
        try { dark = File.Exists(PrefPath) && File.ReadAllText(PrefPath).Trim() == "dark"; }
        catch { /* fall back to light */ }
        Apply(dark, save: false);
    }

    public static void Apply(bool dark, bool save = true)
    {
        // The palette dictionary is always the first merged dictionary (see App.xaml).
        var dictionaries = Application.Current.Resources.MergedDictionaries;
        dictionaries[0] = new ResourceDictionary
        {
            Source = new Uri($"pack://application:,,,/Themes/{(dark ? "Dark" : "Light")}.xaml")
        };
        IsDark = dark;

        if (save)
        {
            try { File.WriteAllText(PrefPath, dark ? "dark" : "light"); }
            catch { /* preference is a convenience only */ }
        }
    }
}
