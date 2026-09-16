using System.Drawing;
using System.Windows.Forms;

namespace Sistem_Data_Mahasiswa;

partial class Form1
{
    private System.ComponentModel.IContainer components = null!;

    // Header & Containers
    private Panel panelHeader = null!;
    private Panel panelLeft = null!;

    // Input Controls (Kiri)
    private Label lblFormTitle = null!;
    private Label lblNRP = null!;
    private Label lblNama = null!;
    private Label lblProdi = null!;
    private Label lblIPK = null!;
    private TextBox txtNRP = null!;
    private TextBox txtNama = null!;
    private TextBox txtProdi = null!;
    private TextBox txtIPK = null!;
    private Button btnTambah = null!;

    // Search & Action Controls (Kanan Atas)
    private Panel panelCari = null!;
    private Label lblCariNRP = null!;
    private TextBox txtCariNRP = null!;
    private Button btnCari = null!;
    private Button btnHapus = null!;

    // Table Data (Kanan Bawah)
    private DataGridView dgvMahasiswa = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        SuspendLayout();

        // 1. Setup Form Utama
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(920, 580);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        Text = "Sistem Data Mahasiswa - Modern Dashboard";
        Name = "Form1";
        BackColor = Color.FromArgb(241, 245, 249);
        Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);

        // 2. Header Top Bar (Teal Accent)
        panelHeader = new Panel
        {
            Dock = DockStyle.Top,
            Height = 60,
            BackColor = Color.FromArgb(13, 148, 136)
        };

        Label lblHeaderTitle = new Label
        {
            Text = "DASHBOARD DATA MAHASISWA",
            Font = new Font("Segoe UI", 15F, FontStyle.Bold),
            ForeColor = Color.White,
            AutoSize = true,
            Location = new Point(20, 15)
        };
        panelHeader.Controls.Add(lblHeaderTitle);

        // 3. Panel Kiri (Form Input Vertikal)
        panelLeft = new Panel
        {
            Location = new Point(20, 80),
            Size = new Size(280, 475),
            BackColor = Color.White,
            BorderStyle = BorderStyle.None
        };

        lblFormTitle = new Label
        {
            Text = "Input Data Baru",
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            ForeColor = Color.FromArgb(15, 23, 42),
            Location = new Point(20, 15),
            AutoSize = true
        };

        Font fontLabel = new Font("Segoe UI", 9F, FontStyle.Bold);
        Font fontInput = new Font("Segoe UI", 9.5F);

        lblNRP = new Label { Text = "NRP / NIM", Location = new Point(20, 55), AutoSize = true, Font = fontLabel, ForeColor = Color.FromArgb(71, 85, 105) };
        txtNRP = new TextBox { Location = new Point(20, 75), Size = new Size(240, 27), Font = fontInput };

        lblNama = new Label { Text = "Nama Lengkap", Location = new Point(20, 120), AutoSize = true, Font = fontLabel, ForeColor = Color.FromArgb(71, 85, 105) };
        txtNama = new TextBox { Location = new Point(20, 140), Size = new Size(240, 27), Font = fontInput };

        lblProdi = new Label { Text = "Program Studi", Location = new Point(20, 185), AutoSize = true, Font = fontLabel, ForeColor = Color.FromArgb(71, 85, 105) };
        txtProdi = new TextBox { Location = new Point(20, 205), Size = new Size(240, 27), Font = fontInput };

        lblIPK = new Label { Text = "IPK (0.00 - 4.00)", Location = new Point(20, 250), AutoSize = true, Font = fontLabel, ForeColor = Color.FromArgb(71, 85, 105) };
        txtIPK = new TextBox { Location = new Point(20, 270), Size = new Size(240, 27), Font = fontInput };

        btnTambah = new Button
        {
            Text = "+ Simpan Data",
            Location = new Point(20, 325),
            Size = new Size(240, 40),
            BackColor = Color.FromArgb(13, 148, 136),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            Cursor = Cursors.Hand
        };
        btnTambah.FlatAppearance.BorderSize = 0;
        btnTambah.Click += BtnTambah_Click;

        panelLeft.Controls.AddRange(new Control[] { lblFormTitle, lblNRP, txtNRP, lblNama, txtNama, lblProdi, txtProdi, lblIPK, txtIPK, btnTambah });

        // 4. Panel Kanan - Area Pencarian & Aksi
        panelCari = new Panel
        {
            Location = new Point(320, 80),
            Size = new Size(580, 55),
            BackColor = Color.White
        };

        lblCariNRP = new Label { Text = "Cari NRP:", Location = new Point(15, 18), AutoSize = true, Font = fontLabel, ForeColor = Color.FromArgb(71, 85, 105) };
        txtCariNRP = new TextBox { Location = new Point(85, 14), Size = new Size(240, 27), Font = fontInput };

        btnCari = new Button
        {
            Text = "Cari",
            Location = new Point(340, 12),
            Size = new Size(100, 32),
            BackColor = Color.FromArgb(2, 132, 199),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            Cursor = Cursors.Hand
        };
        btnCari.FlatAppearance.BorderSize = 0;
        btnCari.Click += BtnCari_Click;

        btnHapus = new Button
        {
            Text = "Hapus",
            Location = new Point(450, 12),
            Size = new Size(110, 32),
            BackColor = Color.FromArgb(225, 29, 72),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            Cursor = Cursors.Hand
        };
        btnHapus.FlatAppearance.BorderSize = 0;
        btnHapus.Click += BtnHapus_Click;

        panelCari.Controls.AddRange(new Control[] { lblCariNRP, txtCariNRP, btnCari, btnHapus });

        // 5. Panel Kanan - DataGridView Table
        dgvMahasiswa = new DataGridView
        {
            Location = new Point(320, 150),
            Size = new Size(580, 405),
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            ReadOnly = true,
            AllowUserToAddRows = false,
            BackgroundColor = Color.White,
            BorderStyle = BorderStyle.None,
            RowHeadersVisible = false
        };

        dgvMahasiswa.EnableHeadersVisualStyles = false;
        dgvMahasiswa.ColumnHeadersHeight = 38;
        dgvMahasiswa.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(15, 23, 42);
        dgvMahasiswa.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        dgvMahasiswa.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        dgvMahasiswa.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dgvMahasiswa.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
        dgvMahasiswa.DefaultCellStyle.SelectionBackColor = Color.FromArgb(204, 251, 241);
        dgvMahasiswa.DefaultCellStyle.SelectionForeColor = Color.FromArgb(15, 23, 42);
        dgvMahasiswa.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);

        // Bind Controls ke Form Utama
        Controls.Add(panelHeader);
        Controls.Add(panelLeft);
        Controls.Add(panelCari);
        Controls.Add(dgvMahasiswa);

        ResumeLayout(false);
    }
}