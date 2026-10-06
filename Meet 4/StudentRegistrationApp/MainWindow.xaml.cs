using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace StudentRegistrationApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly ObservableCollection<Mahasiswa> mahasiswaList = [];

    public MainWindow()
    {
        InitializeComponent();
        lstMahasiswa.ItemsSource = mahasiswaList;
        UpdateCounter();
    }

    private void BtnSimpan_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtNim.Text))
        {
            ShowValidation("NIM harus diisi!", txtNim);
            return;
        }

        if (string.IsNullOrWhiteSpace(txtNama.Text))
        {
            ShowValidation("Nama mahasiswa harus diisi!", txtNama);
            return;
        }

        if (cmbProdi.SelectedItem is not ComboBoxItem selectedProdi)
        {
            MessageBox.Show("Pilih program studi terlebih dahulu.", "Validasi", MessageBoxButton.OK, MessageBoxImage.Warning);
            cmbProdi.Focus();
            return;
        }

        if (rbLaki.IsChecked != true && rbPerempuan.IsChecked != true)
        {
            MessageBox.Show("Pilih jenis kelamin terlebih dahulu.", "Validasi", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        string jenisKelamin = rbLaki.IsChecked == true ? "Laki-laki" : "Perempuan";
        string prodi = selectedProdi.Content?.ToString() ?? string.Empty;

        mahasiswaList.Add(new Mahasiswa(txtNim.Text.Trim(), txtNama.Text.Trim(), prodi, jenisKelamin));
        UpdateCounter();
        MessageBox.Show("Data mahasiswa berhasil disimpan!", "Informasi", MessageBoxButton.OK, MessageBoxImage.Information);
        ResetForm();
    }

    private void BtnReset_Click(object sender, RoutedEventArgs e)
    {
        ResetForm();
    }

    private void BtnHapus_Click(object sender, RoutedEventArgs e)
    {
        if (lstMahasiswa.SelectedItem is not Mahasiswa selectedMahasiswa)
        {
            MessageBox.Show("Pilih data mahasiswa yang ingin dihapus.", "Informasi", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        mahasiswaList.Remove(selectedMahasiswa);
        UpdateCounter();
    }

    private void ResetForm()
    {
        txtNim.Clear();
        txtNama.Clear();
        cmbProdi.SelectedIndex = -1;
        rbLaki.IsChecked = false;
        rbPerempuan.IsChecked = false;
        txtNim.Focus();
    }

    private void UpdateCounter()
    {
        txtCounter.Text = $"{mahasiswaList.Count} mahasiswa";
    }

    private static void ShowValidation(string message, Control control)
    {
        MessageBox.Show(message, "Validasi", MessageBoxButton.OK, MessageBoxImage.Warning);
        control.Focus();
    }
}