using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Sistem_Data_Mahasiswa;

public partial class Form1 : Form
{
    private BindingList<Mahasiswa> listMahasiswa = new BindingList<Mahasiswa>();

    public Form1()
    {
        InitializeComponent();
        InitDataAwal();
    }

    private void InitDataAwal()
    {
        dgvMahasiswa.DataSource = listMahasiswa;

        // Sample data awal
        listMahasiswa.Add(new Mahasiswa("5025231288", "Leyan Harits R W", "Teknik Informatika", 3.5));

        // Menggunakan pattern matching 'is DataGridViewColumn' untuk menghindari warning CS8602
        if (dgvMahasiswa.Columns["IPK"] is DataGridViewColumn colIPK)
        {
            colIPK.DefaultCellStyle.Format = "N2";
            colIPK.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        if (dgvMahasiswa.Columns["NRP"] is DataGridViewColumn colNRP)
        {
            colNRP.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }
    }

    private void BtnTambah_Click(object? sender, EventArgs e)
    {
        string nrp = txtNRP.Text.Trim();
        string nama = txtNama.Text.Trim();
        string prodi = txtProdi.Text.Trim();

        if (string.IsNullOrEmpty(nrp) || string.IsNullOrEmpty(nama) || string.IsNullOrEmpty(prodi) || string.IsNullOrEmpty(txtIPK.Text))
        {
            MessageBox.Show("Semua kolom input wajib diisi!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (listMahasiswa.Any(m => m.NRP.Equals(nrp, StringComparison.OrdinalIgnoreCase)))
        {
            MessageBox.Show("Mahasiswa dengan NRP tersebut sudah terdaftar!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (!double.TryParse(txtIPK.Text, out double ipk) || ipk < 0.0 || ipk > 4.0)
        {
            MessageBox.Show("IPK harus berupa angka antara 0.00 hingga 4.00!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        listMahasiswa.Add(new Mahasiswa(nrp, nama, prodi, ipk));
        MessageBox.Show("Data mahasiswa berhasil ditambahkan!", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        ClearInputForm();
    }

    private void BtnCari_Click(object? sender, EventArgs e)
    {
        string keyword = txtCariNRP.Text.Trim();
        if (string.IsNullOrEmpty(keyword))
        {
            MessageBox.Show("Masukkan NRP yang ingin dicari!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var mhs = listMahasiswa.FirstOrDefault(m => m.NRP.Equals(keyword, StringComparison.OrdinalIgnoreCase));
        if (mhs != null)
        {
            int index = listMahasiswa.IndexOf(mhs);
            dgvMahasiswa.ClearSelection();
            dgvMahasiswa.Rows[index].Selected = true;
            dgvMahasiswa.FirstDisplayedScrollingRowIndex = index;
            MessageBox.Show($"Data Ditemukan:\nNama: {mhs.Nama}\nProdi: {mhs.Prodi}\nIPK: {mhs.IPK:F2}", "Pencarian Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        else
        {
            MessageBox.Show($"Mahasiswa dengan NRP '{keyword}' tidak ditemukan.", "Pencarian", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private void BtnHapus_Click(object? sender, EventArgs e)
    {
        string keyword = txtCariNRP.Text.Trim();
        if (string.IsNullOrEmpty(keyword))
        {
            MessageBox.Show("Masukkan NRP yang ingin dihapus!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var mhs = listMahasiswa.FirstOrDefault(m => m.NRP.Equals(keyword, StringComparison.OrdinalIgnoreCase));
        if (mhs != null)
        {
            var confirm = MessageBox.Show($"Yakin ingin menghapus data {mhs.Nama} ({mhs.NRP})?", "Konfirmasi Hapus", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                listMahasiswa.Remove(mhs);
                txtCariNRP.Clear();
                MessageBox.Show("Data mahasiswa berhasil dihapus!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        else
        {
            MessageBox.Show($"Mahasiswa dengan NRP '{keyword}' tidak ditemukan.", "Hapus Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ClearInputForm()
    {
        txtNRP.Clear();
        txtNama.Clear();
        txtProdi.Clear();
        txtIPK.Clear();
        txtNRP.Focus();
    }
}