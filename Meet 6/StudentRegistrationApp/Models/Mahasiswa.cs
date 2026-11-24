namespace StudentRegistrationApp.Models;

public sealed class Mahasiswa
{
    public int StudentId { get; set; }
    public string Nim { get; set; } = string.Empty;
    public string Nama { get; set; } = string.Empty;
    public int ProgramId { get; set; }
    public string ProgramName { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string Alamat { get; set; } = string.Empty;
    public string NoTelepon { get; set; } = string.Empty;
    public DateTime Dibuat { get; set; }
}
