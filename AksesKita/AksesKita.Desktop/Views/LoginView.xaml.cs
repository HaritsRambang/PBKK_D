using System.Windows.Controls;
using AksesKita.Desktop.ViewModels;

namespace AksesKita.Desktop.Views;

public partial class LoginView : UserControl
{
    private LoginViewModel? _vm;

    public LoginView()
    {
        InitializeComponent();

        // PasswordBox.Password is not a dependency property, so it is copied into the view-model here.
        PwdBox.PasswordChanged += (_, _) => { if (_vm != null) _vm.Password = PwdBox.Password; };
        ConfirmBox.PasswordChanged += (_, _) => { if (_vm != null) _vm.ConfirmPassword = ConfirmBox.Password; };

        DataContextChanged += (_, e) =>
        {
            if (_vm != null) _vm.ClearPasswords -= OnClearPasswords;
            _vm = e.NewValue as LoginViewModel;
            if (_vm != null) _vm.ClearPasswords += OnClearPasswords;
        };

        Loaded += (_, _) => EmailBox.Focus();
    }

    private void OnClearPasswords()
    {
        PwdBox.Clear();
        ConfirmBox.Clear();
    }
}
