using System.Windows;
using StudentRegistrationApp.Data;
using StudentRegistrationApp.ViewModels;

namespace StudentRegistrationApp;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MahasiswaViewModel(new MahasiswaRepository());
    }
}