# Tugas Meet 3 - Calculator Sakti

Aplikasi ini adalah **Calculator Sakti**, kalkulator desktop berbasis **Windows Forms** menggunakan C# dan .NET 10. Project ini memenuhi studi kasus praktikum NET Programming dan menambahkan pengembangan sederhana dari bagian tantangan pada panduan.

## Fitur

- Penjumlahan, pengurangan, perkalian, dan pembagian.
- Bilangan desimal dengan tombol `.`.
- Scientific calculator sederhana: `sin`, `cos`, `tan`, `sqrt`, kuadrat `x²`, dan perubahan tanda `±`.
- Persentase sederhana melalui tombol `%`.
- Clear (`C`) dan hapus satu karakter (`Back`).
- Riwayat maksimal delapan perhitungan melalui tombol `HISTORY`.
- Validasi pembagian dengan nol.
- Dukungan keyboard untuk angka, numpad, operator, `Enter`, `Escape`, `Backspace`, dan tombol desimal.
- Status kecil pada form untuk memberi umpan balik tanpa mengganggu pengguna.

## Struktur Folder

```text
Meet 3/
├── .gitignore
├── Images/
│   ├── Screenshot 2026-10-04 011923.png
│   └── Screenshot 2026-10-04 012231.png
├── README.md
└── CalculatorApp/
    ├── CalculatorApp.csproj
    ├── Program.cs
    ├── Form1.cs
    └── Form1.Designer.cs
```

Folder `Images` berisi dokumentasi screenshot aplikasi dan sengaja tetap disertakan dalam repository.

## Dokumentasi Tampilan

![Tampilan utama Calculator Sakti](Images/Screenshot%202026-10-04%20011923.png)

![Tampilan history Calculator Sakti](Images/Screenshot%202026-10-04%20012231.png)

## Konsep yang Digunakan

1. `Form1.Designer.cs` menyiapkan komponen form, warna, layout grid, display, dan label status.
2. `Form1.cs` menangani event tombol, input keyboard, penyimpanan operand pertama, operator, dan perhitungan.
3. `decimal` digunakan agar operasi desimal lebih tepat dibandingkan `double` untuk kasus kalkulator dasar.
4. `switch expression` pada method `Calculate` memilih operasi berdasarkan operator yang sedang aktif.
5. Fungsi scientific menggunakan class `Math`; `sin`, `cos`, dan `tan` menerima sudut dalam derajat agar mudah digunakan.
6. `TryParse` memvalidasi input display sebelum perhitungan.
7. `DivideByZeroException` ditangani sehingga aplikasi tetap berjalan ketika pembagi bernilai nol.

