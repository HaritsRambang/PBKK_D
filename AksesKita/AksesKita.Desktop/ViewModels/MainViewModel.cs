using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AksesKita.Desktop.Models;
using AksesKita.Desktop.Services;

namespace AksesKita.Desktop.ViewModels;

public partial class MainViewModel : ObservableObject, INavigator
{
    private static readonly double[] Scales = { 1.0, 1.12, 1.25 };
    private int _scaleIndex;

    [ObservableProperty] private ObservableObject _currentView;
    [ObservableProperty] private string _currentPage = "Login";
    [ObservableProperty] private bool _isLoggedIn;
    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private double _uiScale = 1.0;

    public AppSession Session { get; } = new();

    public LoginViewModel LoginVM { get; }
    public DashboardViewModel DashboardVM { get; }
    public ReportListViewModel ReportListVM { get; }
    public CreateReportViewModel CreateReportVM { get; }
    public ReportDetailViewModel ReportDetailVM { get; }
    public MapViewModel MapVM { get; }

    public MainViewModel() : this(new ReportService()) { }

    public MainViewModel(IReportService service)
    {
        LoginVM = new LoginViewModel(this);
        DashboardVM = new DashboardViewModel(service, this);
        ReportListVM = new ReportListViewModel(service, this);
        CreateReportVM = new CreateReportViewModel(service, this);
        ReportDetailVM = new ReportDetailViewModel(service, this);
        MapVM = new MapViewModel(service, this);
        _currentView = LoginVM;
    }

    public void Login(string name, UserRole role)
    {
        Session.UserName = string.IsNullOrWhiteSpace(name) ? (role == UserRole.Admin ? "Admin" : "Warga") : name.Trim();
        Session.Role = role;
        IsLoggedIn = true;
        Navigate("Dashboard");
    }

    [RelayCommand]
    private void Logout()
    {
        IsLoggedIn = false;
        Session.UserName = string.Empty;
        SearchText = string.Empty;
        LoginVM.Name = string.Empty;
        CurrentPage = "Login";
        CurrentView = LoginVM;
    }

    /// <summary>Cycles through three text/UI sizes (accessibility option from the design).</summary>
    [RelayCommand]
    private void CycleScale()
    {
        _scaleIndex = (_scaleIndex + 1) % Scales.Length;
        UiScale = Scales[_scaleIndex];
    }

    [RelayCommand]
    private void Search()
    {
        ReportListVM.SearchKeyword = SearchText.Trim();
        Navigate("ReportList");
    }

    [RelayCommand]
    public void Navigate(string destination)
    {
        CurrentPage = destination;
        switch (destination)
        {
            case "Dashboard":
                _ = DashboardVM.LoadAsync();
                CurrentView = DashboardVM;
                break;
            case "Map":
                _ = MapVM.LoadAsync();
                CurrentView = MapVM;
                break;
            case "ReportList":
                _ = ReportListVM.LoadAsync();
                CurrentView = ReportListVM;
                break;
            case "CreateReport":
                CreateReportVM.Reset();
                _ = CreateReportVM.LoadCategoriesAsync();
                CurrentView = CreateReportVM;
                break;
            case "ReportDetail":
                // Reopens the last viewed ticket, or the newest one when none was opened yet.
                _ = ReportDetailVM.LoadAsync(ReportDetailVM.Report?.Id ?? 0);
                CurrentView = ReportDetailVM;
                break;
            default:
                _ = DashboardVM.LoadAsync();
                CurrentView = DashboardVM;
                break;
        }
    }

    public void ShowReportDetail(int reportId)
    {
        _ = ReportDetailVM.LoadAsync(reportId);
        CurrentPage = "ReportDetail";
        CurrentView = ReportDetailVM;
    }

    public void ShowOnMap(int reportId)
    {
        MapVM.PendingFocusId = reportId;
        Navigate("Map");
    }
}
