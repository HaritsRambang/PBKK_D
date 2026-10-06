namespace StudentRegistrationApp;

public sealed class Mahasiswa
{
    public Mahasiswa(string nim, string nama, string prodi, string jenisKelamin)
    {
        Nim = nim;
        Nama = nama;
        Prodi = prodi;
        JenisKelamin = jenisKelamin;
    }

    public string Nim { get; }
    public string Nama { get; }
    public string Prodi { get; }
    public string JenisKelamin { get; }
    public string DisplayText => $"{Nim} | {Nama} | {Prodi} | {JenisKelamin}";
}