namespace StudentRegistrationApp.Models;

public sealed class Mahasiswa
{
    public Mahasiswa(string nim, string nama, string prodi, string gender)
    {
        Nim = nim;
        Nama = nama;
        Prodi = prodi;
        Gender = gender;
    }

    public string Nim { get; }
    public string Nama { get; }
    public string Prodi { get; }
    public string Gender { get; }

    public bool Matches(string search)
    {
        return string.IsNullOrWhiteSpace(search)
            || Nim.Contains(search, StringComparison.OrdinalIgnoreCase)
            || Nama.Contains(search, StringComparison.OrdinalIgnoreCase)
            || Prodi.Contains(search, StringComparison.OrdinalIgnoreCase);
    }
}