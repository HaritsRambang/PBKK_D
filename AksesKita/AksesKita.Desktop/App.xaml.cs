using System;
using System.IO;
using System.Windows;
using AksesKita.Desktop.Data;
using AksesKita.Desktop.Services;
using AksesKita.Desktop.ViewModels;
using AksesKita.Desktop.Views;

namespace AksesKita.Desktop;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Write unexpected errors to a log file instead of silently closing the app.
        DispatcherUnhandledException += (_, args) =>
        {
            LogError(args.Exception);
            MessageBox.Show($"Terjadi kesalahan:\n{args.Exception.Message}\n\nDetail tersimpan di crash.log.",
                "AksesKita", MessageBoxButton.OK, MessageBoxImage.Error);
            args.Handled = true;
        };

        // Create the SQLite database and seed starter data on first run.
        AksesKitaDbContext.Initialize();

        var mainVm = new MainViewModel(new ReportService());
        var window = new MainWindow { DataContext = mainVm };
        window.Show();
    }

    private static void LogError(Exception ex)
    {
        try
        {
            File.AppendAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "crash.log"),
                $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {ex}\n\n");
        }
        catch { /* never throw from the error handler */ }
    }
}
