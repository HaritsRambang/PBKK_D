using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AksesKita.Desktop.Models;

namespace AksesKita.Desktop.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly MainViewModel _main;

    [ObservableProperty] private string _name = string.Empty;

    public LoginViewModel(MainViewModel main) => _main = main;

    [RelayCommand]
    private void MasukWarga() => _main.Login(Name, UserRole.Pelapor);

    [RelayCommand]
    private void MasukAdmin() => _main.Login(Name, UserRole.Admin);
}
