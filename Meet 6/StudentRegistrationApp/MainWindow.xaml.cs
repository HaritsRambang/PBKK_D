using System.Windows;
using StudentRegistrationApp.Data;

namespace StudentRegistrationApp;

public partial class MainWindow : Window
{
    private readonly MahasiswaRepository repository = new();

    public MainWindow()
    {
        InitializeComponent();
        LoadStudents();
    }

    private void RefreshButton_Click(object sender, RoutedEventArgs e)
    {
        LoadStudents();
    }

    private void LoadStudents()
    {
        try
        {
            var students = repository.GetAll();
            int total = repository.GetTotal();

            StudentsDataGrid.ItemsSource = students;
            TotalStudentsText.Text = $"Total Mahasiswa: {total}";
            StatusText.Text = $"Status: Terhubung ({DateTime.Now:HH:mm:ss})";
        }
        catch (Exception ex)
        {
            StatusText.Text = "Status: Koneksi Gagal";
            MessageBox.Show(
                $"Gagal memuat data dari SQL Server.\n\nDetail: {ex.Message}",
                "Database Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }
}
