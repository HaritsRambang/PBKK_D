# Sistem Data Mahasiswa

Aplikasi desktop Windows Forms untuk mengelola data mahasiswa secara sederhana. Data yang dikelola meliputi NRP, nama, program studi, dan IPK.

## Fitur

- Menampilkan data mahasiswa pada tabel
- Menambahkan data mahasiswa baru
- Mencari mahasiswa berdasarkan NRP
- Menghapus data mahasiswa berdasarkan NRP
- Validasi NRP duplikat dan IPK pada rentang 0.00 sampai 4.00

## Persyaratan

- Windows
- .NET SDK 10.0 atau lebih baru

## Menjalankan aplikasi

Dari folder proyek ini, jalankan:

```bash
dotnet run
```

Aplikasi Windows Forms akan terbuka sebagai jendela desktop.

## Build

```bash
dotnet build
```

Data saat ini disimpan selama aplikasi berjalan dan belum menggunakan database.
