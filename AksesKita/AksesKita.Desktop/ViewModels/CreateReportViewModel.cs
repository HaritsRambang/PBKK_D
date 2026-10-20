using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AksesKita.Desktop.Models;
using AksesKita.Desktop.Services;

namespace AksesKita.Desktop.ViewModels;

public record SeverityOption(SeverityLevel Level, string Title, string Description, string Badge);

public record Landmark(string Name, string Address, double Latitude, double Longitude);

public partial class CreateReportViewModel : ObservableObject
{
    private readonly IReportService _service;
    private readonly INavigator _nav;

    [ObservableProperty, NotifyPropertyChangedFor(nameof(TitleCounter))] private string _title = string.Empty;
    [ObservableProperty, NotifyPropertyChangedFor(nameof(DescriptionCounter))] private string _description = string.Empty;
    [ObservableProperty] private string _address = string.Empty;
    [ObservableProperty] private bool _isAnonymous;
    [ObservableProperty] private Category? _selectedCategory;
    [ObservableProperty] private SeverityOption? _selectedSeverity;
    [ObservableProperty] private Landmark? _selectedLandmark;
    [ObservableProperty] private string _latitudeText = "-7.2575";
    [ObservableProperty] private string _longitudeText = "112.7521";
    [ObservableProperty] private string? _imagePath;
    [ObservableProperty] private string _statusMessage = string.Empty;

    public ObservableCollection<Category> Categories { get; } = new();

    public SeverityOption[] SeverityOptions { get; } =
    {
        new(SeverityLevel.Rendah, "Rendah — Masih Bisa Dilalui", "Kondisi mengganggu kelancaran, namun pengguna kursi roda atau tongkat masih dapat melewatinya dengan kehati-hatian ekstra.", "Prioritas Normal"),
        new(SeverityLevel.Sedang, "Sedang — Butuh Bantuan Pendamping", "Hambatan tidak dapat dilalui sendiri oleh disabilitas berat/pengguna kursi roda tanpa bantuan orang lain.", "Perhatian Khusus"),
        new(SeverityLevel.Kritis, "Kritis — Total Blokir / Membahayakan Jiwa", "Akses terputus sepenuhnya, memaksa pejalan kaki turun ke jalan raya bermotor, atau berisiko tinggi jatuh/cedera fisik.", "Eskalasi Cepat"),
    };

    /// <summary>Quick-pick landmarks in Surabaya that fill the address and coordinates.</summary>
    public Landmark[] Landmarks { get; } =
    {
        new("Tunjungan Plaza", "Jl. Basuki Rahmat No. 8-12, Surabaya", -7.2625, 112.7403),
        new("Stasiun Gubeng", "Jl. Gubeng Masjid, Surabaya", -7.2652, 112.7524),
        new("Stasiun Pasar Turi", "Jl. Semarang, Surabaya", -7.2460, 112.7295),
        new("Balai Kota Surabaya", "Jl. Taman Surya No. 1, Surabaya", -7.2587, 112.7508),
        new("Taman Bungkul", "Jl. Raya Darmo, Surabaya", -7.2913, 112.7396),
        new("Delta Plaza", "Jl. Pemuda No. 31-37, Surabaya", -7.2631, 112.7468),
        new("Jembatan Merah Plaza", "Jl. Jembatan Merah, Surabaya", -7.2332, 112.7366),
        new("RSUD Dr. Soetomo", "Jl. Mayjen Prof. Dr. Moestopo, Surabaya", -7.2689, 112.7584),
        new("Kampus ITS Sukolilo", "Jl. Raya ITS, Sukolilo, Surabaya", -7.2819, 112.7948),
        new("Universitas Airlangga (Kampus B)", "Jl. Dharmawangsa Dalam, Surabaya", -7.2699, 112.7578),
        new("Terminal Joyoboyo", "Jl. Joyoboyo, Surabaya", -7.2994, 112.7387),
        new("Royal Plaza Wonokromo", "Jl. Ahmad Yani, Surabaya", -7.3077, 112.7342),
    };

    public string TitleCounter => $"{Title.Length} / 100 karakter";
    public string DescriptionCounter => $"{Description.Length} karakter (min. 20)";

    public CreateReportViewModel(IReportService service, INavigator nav)
    {
        _service = service;
        _nav = nav;
        _selectedSeverity = SeverityOptions[1];
    }

    public async Task LoadCategoriesAsync()
    {
        if (Categories.Count == 0)
        {
            foreach (var c in await _service.GetCategoriesAsync()) Categories.Add(c);
        }
        SelectedCategory ??= Categories.FirstOrDefault();
    }

    public void Reset()
    {
        Title = Description = Address = string.Empty;
        SelectedSeverity = SeverityOptions[1];
        SelectedLandmark = null;
        LatitudeText = "-7.2575";
        LongitudeText = "112.7521";
        IsAnonymous = false;
        ImagePath = null;
        StatusMessage = string.Empty;
        SelectedCategory = Categories.FirstOrDefault();
    }

    partial void OnSelectedLandmarkChanged(Landmark? value)
    {
        if (value == null) return;
        Address = value.Address;
        LatitudeText = value.Latitude.ToString(CultureInfo.InvariantCulture);
        LongitudeText = value.Longitude.ToString(CultureInfo.InvariantCulture);
    }

    [RelayCommand]
    private void PickPhoto()
    {
        var dlg = new Microsoft.Win32.OpenFileDialog
        {
            Filter = "Gambar (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png",
            Title = "Pilih foto bukti hambatan"
        };
        if (dlg.ShowDialog() != true) return;

        var info = new FileInfo(dlg.FileName);
        if (info.Length > 5 * 1024 * 1024)
        {
            StatusMessage = "⚠ Ukuran foto maksimal 5 MB.";
            return;
        }

        var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Uploads");
        Directory.CreateDirectory(dir);
        var dest = Path.Combine(dir, $"{Guid.NewGuid():N}{info.Extension}");
        File.Copy(info.FullName, dest);
        ImagePath = dest;
        StatusMessage = string.Empty;
    }

    [RelayCommand]
    private void RemovePhoto() => ImagePath = null;

    [RelayCommand]
    private async Task SubmitReport()
    {
        if (SelectedCategory == null)
        {
            StatusMessage = "⚠ Pilih kategori fasilitas yang terhambat.";
            return;
        }
        if (Title.Trim().Length < 5)
        {
            StatusMessage = "⚠ Judul hambatan minimal 5 karakter.";
            return;
        }
        if (Description.Trim().Length < 20)
        {
            StatusMessage = "⚠ Deskripsi minimal 20 karakter agar petugas dapat memahami kondisinya.";
            return;
        }
        if (string.IsNullOrWhiteSpace(Address))
        {
            StatusMessage = "⚠ Alamat atau patokan lokasi wajib diisi.";
            return;
        }
        if (!double.TryParse(LatitudeText, NumberStyles.Float, CultureInfo.InvariantCulture, out var lat) || lat < -90 || lat > 90 ||
            !double.TryParse(LongitudeText, NumberStyles.Float, CultureInfo.InvariantCulture, out var lng) || lng < -180 || lng > 180)
        {
            StatusMessage = "⚠ Koordinat tidak valid. Gunakan format desimal, contoh -7.2575 dan 112.7521.";
            return;
        }

        var report = new Report
        {
            Title = Title.Trim(),
            Description = Description.Trim(),
            Address = Address.Trim(),
            Severity = SelectedSeverity?.Level ?? SeverityLevel.Sedang,
            IsAnonymous = IsAnonymous,
            CategoryId = SelectedCategory.Id,
            Latitude = lat,
            Longitude = lng,
            ImagePath = ImagePath,
            ReporterName = _nav.Session.UserName
        };

        var saved = await _service.AddReportAsync(report);
        Reset();
        _nav.ShowReportDetail(saved.Id);
    }
}
