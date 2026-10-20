using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AksesKita.Desktop.Data;
using AksesKita.Desktop.Models;

namespace AksesKita.Desktop.Services;

/// <summary>Account registration and sign-in. Passwords are never stored, only their BCrypt hash.</summary>
public class AuthService
{
    private static readonly Regex EmailPattern = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

    public async Task<AppUser?> LoginAsync(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrEmpty(password)) return null;

        var key = email.Trim().ToLowerInvariant();
        using var db = new AksesKitaDbContext();
        var user = await db.Users.FirstOrDefaultAsync(u => u.Email == key);
        if (user == null) return null;

        // BCrypt is deliberately slow; keep it off the UI thread.
        var ok = await Task.Run(() => BCrypt.Net.BCrypt.Verify(password, user.PasswordHash));
        return ok ? user : null;
    }

    /// <summary>Self-registration always creates a citizen (Pelapor) account; admins are provisioned separately.</summary>
    public async Task<(AppUser? User, string? Error)> RegisterAsync(string name, string email, string password, string confirm)
    {
        name = name.Trim();
        var key = email.Trim().ToLowerInvariant();

        if (name.Length < 3) return (null, "Nama minimal 3 karakter.");
        if (!EmailPattern.IsMatch(key)) return (null, "Format email tidak valid.");
        if (password.Length < 8) return (null, "Kata sandi minimal 8 karakter.");
        if (!password.Any(char.IsLetter) || !password.Any(char.IsDigit))
            return (null, "Kata sandi harus berisi kombinasi huruf dan angka.");
        if (password != confirm) return (null, "Konfirmasi kata sandi tidak sama.");

        using var db = new AksesKitaDbContext();
        if (await db.Users.AnyAsync(u => u.Email == key))
            return (null, "Email sudah terdaftar. Silakan masuk.");

        var hash = await Task.Run(() => BCrypt.Net.BCrypt.HashPassword(password));
        var user = new AppUser
        {
            Name = name,
            Email = key,
            PasswordHash = hash,
            Role = UserRole.Pelapor,
            CreatedAt = DateTime.Now
        };
        db.Users.Add(user);
        await db.SaveChangesAsync();
        return (user, null);
    }
}
