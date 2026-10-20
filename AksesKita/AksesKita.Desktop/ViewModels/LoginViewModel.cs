using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AksesKita.Desktop.Services;

namespace AksesKita.Desktop.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly MainViewModel _main;
    private readonly AuthService _auth;

    [ObservableProperty,
     NotifyPropertyChangedFor(nameof(Heading)), NotifyPropertyChangedFor(nameof(SubmitLabel)),
     NotifyPropertyChangedFor(nameof(ToggleLabel))]
    private bool _isRegisterMode;

    [ObservableProperty] private string _name = string.Empty;
    [ObservableProperty] private string _email = string.Empty;
    [ObservableProperty] private string _errorMessage = string.Empty;
    [ObservableProperty] private bool _isBusy;

    // PasswordBox does not support binding, so the view pushes these in from code-behind.
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;

    /// <summary>Asks the view to empty its password boxes.</summary>
    public event Action? ClearPasswords;

    public string Heading => IsRegisterMode ? "Buat akun warga baru" : "Masuk ke akun Anda";
    public string SubmitLabel => IsRegisterMode ? "Daftar & Masuk" : "Masuk";
    public string ToggleLabel => IsRegisterMode ? "Sudah punya akun? Masuk" : "Belum punya akun? Daftar sebagai warga";

    public LoginViewModel(MainViewModel main, AuthService auth)
    {
        _main = main;
        _auth = auth;
    }

    [RelayCommand]
    private async Task Submit()
    {
        if (IsBusy) return;
        ErrorMessage = string.Empty;
        IsBusy = true;
        try
        {
            if (IsRegisterMode)
            {
                var (user, error) = await _auth.RegisterAsync(Name, Email, Password, ConfirmPassword);
                if (user == null) { ErrorMessage = "⚠ " + error; return; }
                _main.Login(user);
            }
            else
            {
                var user = await _auth.LoginAsync(Email, Password);
                if (user == null) { ErrorMessage = "⚠ Email atau kata sandi salah."; return; }
                _main.Login(user);
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = "⚠ Gagal memproses: " + ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private void ToggleMode()
    {
        IsRegisterMode = !IsRegisterMode;
        ErrorMessage = string.Empty;
    }

    public void Reset()
    {
        Name = Email = ErrorMessage = string.Empty;
        Password = ConfirmPassword = string.Empty;
        IsRegisterMode = false;
        ClearPasswords?.Invoke();
    }
}
