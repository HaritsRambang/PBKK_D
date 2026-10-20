using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AksesKita.Desktop.Models;
using AksesKita.Desktop.Services;

namespace AksesKita.Desktop.ViewModels;

public partial class ReportListViewModel : ObservableObject
{
    private readonly IReportService _service;
    private readonly INavigator _nav;
    private List<Report> _all = new();

    [ObservableProperty] private ObservableCollection<Report> _reports = new();
    [ObservableProperty] private string _searchKeyword = string.Empty;
    [ObservableProperty] private string _statusFilter = "Semua";
    [ObservableProperty] private string _severityFilter = "Semua";
    [ObservableProperty] private bool _onlyMine;
    [ObservableProperty] private bool _isAdmin;
    [ObservableProperty] private string _summary = string.Empty;

    public string[] StatusFilters { get; } = { "Semua", "Baru", "Terverifikasi", "Dalam Perbaikan", "Selesai", "Ditolak" };
    public string[] SeverityFilters { get; } = { "Semua", "Kritis", "Sedang", "Rendah" };

    public ReportListViewModel(IReportService service, INavigator nav)
    {
        _service = service;
        _nav = nav;
    }

    public async Task LoadAsync()
    {
        IsAdmin = _nav.Session.IsAdmin;
        _all = await _service.GetAllReportsAsync();
        Apply();
    }

    partial void OnSearchKeywordChanged(string value) => Apply();
    partial void OnStatusFilterChanged(string value) => Apply();
    partial void OnSeverityFilterChanged(string value) => Apply();
    partial void OnOnlyMineChanged(bool value) => Apply();

    private void Apply()
    {
        IEnumerable<Report> q = _all;

        if (!string.IsNullOrWhiteSpace(SearchKeyword))
        {
            var k = SearchKeyword.Trim();
            q = q.Where(r => Contains(r.Title, k) || Contains(r.Address, k) || Contains(r.TicketCode, k)
                          || Contains(r.Description, k) || Contains(r.Category?.Name, k));
        }
        if (StatusFilter != "Semua" && Enum.TryParse<ReportStatus>(StatusFilter.Replace(" ", ""), out var st))
            q = q.Where(r => r.Status == st);
        if (SeverityFilter != "Semua" && Enum.TryParse<SeverityLevel>(SeverityFilter, out var sv))
            q = q.Where(r => r.Severity == sv);
        if (OnlyMine)
            q = q.Where(r => r.ReporterName == _nav.Session.UserName);

        var list = q.ToList();
        Reports = new ObservableCollection<Report>(list);
        Summary = $"Menampilkan {list.Count} dari {_all.Count} laporan";
    }

    private static bool Contains(string? text, string keyword)
        => text != null && text.Contains(keyword, StringComparison.OrdinalIgnoreCase);

    [RelayCommand]
    private void ResetFilter()
    {
        SearchKeyword = string.Empty;
        StatusFilter = "Semua";
        SeverityFilter = "Semua";
        OnlyMine = false;
    }

    [RelayCommand]
    private void OpenDetail(Report? report)
    {
        if (report != null) _nav.ShowReportDetail(report.Id);
    }

    [RelayCommand]
    private void NewReport() => _nav.Navigate("CreateReport");

    [RelayCommand]
    private async Task DeleteReport(Report? report)
    {
        if (report == null) return;
        var ok = System.Windows.MessageBox.Show(
            $"Hapus laporan \"{report.Title}\"?", "Konfirmasi Hapus",
            System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Warning);
        if (ok == System.Windows.MessageBoxResult.Yes)
        {
            await _service.DeleteReportAsync(report.Id);
            _all.RemoveAll(r => r.Id == report.Id);
            Apply();
        }
    }
}
