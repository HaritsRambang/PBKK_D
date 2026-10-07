using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using StudentRegistrationApp.Data;
using StudentRegistrationApp.Models;

namespace StudentRegistrationApp.ViewModels;

public sealed class MahasiswaViewModel : INotifyPropertyChanged
{
    private readonly MahasiswaRepository repository;
    private string nim = string.Empty;
    private string nama = string.Empty;
    private string? prodi;
    private string? gender;
    private string searchText = string.Empty;
    private Mahasiswa? selectedStudent;

    public MahasiswaViewModel(MahasiswaRepository repository)
    {
        this.repository = repository;
        SaveCommand = new RelayCommand(_ => Save());
        UpdateCommand = new RelayCommand(_ => Update());
        DeleteCommand = new RelayCommand(_ => Delete());
        ResetCommand = new RelayCommand(_ => Reset());
        RefreshVisibleStudents();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<Mahasiswa> VisibleStudents { get; } = [];

    public IReadOnlyList<string> Programs { get; } =
    [
        "Teknik Informatika",
        "Sistem Informasi",
        "Manajemen",
        "Akuntansi"
    ];

    public IReadOnlyList<string> Genders { get; } = ["Laki-laki", "Perempuan"];

    public string Nim { get => nim; set => SetField(ref nim, value); }
    public string Nama { get => nama; set => SetField(ref nama, value); }
    public string? Prodi { get => prodi; set => SetField(ref prodi, value); }
    public string? Gender { get => gender; set => SetField(ref gender, value); }

    public string SearchText
    {
        get => searchText;
        set
        {
            if (SetField(ref searchText, value))
            {
                RefreshVisibleStudents();
            }
        }
    }

    public Mahasiswa? SelectedStudent
    {
        get => selectedStudent;
        set
        {
            if (!SetField(ref selectedStudent, value) || value is null)
            {
                return;
            }

            Nim = value.Nim;
            Nama = value.Nama;
            Prodi = value.Prodi;
            Gender = value.Gender;
        }
    }

    public string CounterText => $"Jumlah mahasiswa: {repository.GetAll().Count}";
    public ICommand SaveCommand { get; }
    public ICommand UpdateCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand ResetCommand { get; }

    private void Save()
    {
        if (!ValidateForm() || repository.ExistsNim(Nim.Trim()))
        {
            if (repository.ExistsNim(Nim.Trim()))
            {
                ShowMessage("NIM sudah terdaftar.", "Validasi", MessageBoxImage.Warning);
            }

            return;
        }

        repository.Add(CreateStudent());
        RefreshVisibleStudents();
        ShowMessage("Data mahasiswa berhasil disimpan.", "Informasi", MessageBoxImage.Information);
        Reset();
    }

    private void Update()
    {
        if (SelectedStudent is null)
        {
            ShowMessage("Pilih data pada tabel yang ingin diubah.", "Informasi", MessageBoxImage.Warning);
            return;
        }

        if (!ValidateForm())
        {
            return;
        }

        repository.Update(SelectedStudent, CreateStudent());
        RefreshVisibleStudents();
        ShowMessage("Data mahasiswa berhasil diperbarui.", "Informasi", MessageBoxImage.Information);
        Reset();
    }

    private void Delete()
    {
        if (SelectedStudent is null)
        {
            ShowMessage("Pilih data yang ingin dihapus.", "Informasi", MessageBoxImage.Warning);
            return;
        }

        MessageBoxResult confirmation = MessageBox.Show("Hapus data mahasiswa terpilih?", "Konfirmasi", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (confirmation == MessageBoxResult.Yes)
        {
            repository.Remove(SelectedStudent);
            RefreshVisibleStudents();
            Reset();
        }
    }

    private void Reset()
    {
        SelectedStudent = null;
        Nim = string.Empty;
        Nama = string.Empty;
        Prodi = null;
        Gender = null;
    }

    private bool ValidateForm()
    {
        if (string.IsNullOrWhiteSpace(Nim))
        {
            ShowMessage("NIM harus diisi.", "Validasi", MessageBoxImage.Warning);
            return false;
        }

        if (string.IsNullOrWhiteSpace(Nama))
        {
            ShowMessage("Nama mahasiswa harus diisi.", "Validasi", MessageBoxImage.Warning);
            return false;
        }

        if (string.IsNullOrWhiteSpace(Prodi))
        {
            ShowMessage("Pilih program studi.", "Validasi", MessageBoxImage.Warning);
            return false;
        }

        if (string.IsNullOrWhiteSpace(Gender))
        {
            ShowMessage("Pilih jenis kelamin.", "Validasi", MessageBoxImage.Warning);
            return false;
        }

        return true;
    }

    private Mahasiswa CreateStudent()
    {
        return new Mahasiswa(Nim.Trim(), Nama.Trim(), Prodi!, Gender!);
    }

    private void RefreshVisibleStudents()
    {
        VisibleStudents.Clear();
        foreach (Mahasiswa student in repository.GetAll().Where(student => student.Matches(SearchText)))
        {
            VisibleStudents.Add(student);
        }

        OnPropertyChanged(nameof(CounterText));
    }

    private static void ShowMessage(string message, string title, MessageBoxImage image)
    {
        MessageBox.Show(message, title, MessageBoxButton.OK, image);
    }

    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return false;
        }

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}