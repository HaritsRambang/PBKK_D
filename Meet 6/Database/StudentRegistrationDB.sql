-- ============================================================================
-- Meet 6 - Hands-On Lab 1: StudentRegistrationDB (SQL Server)
-- ============================================================================

-- 1. Membuat dan menggunakan database
IF DB_ID('StudentRegistrationDB') IS NULL
BEGIN
    CREATE DATABASE StudentRegistrationDB;
END
GO

USE StudentRegistrationDB;
GO

-- 2. Membuat tabel Programs (Master Program Studi)
CREATE TABLE Programs (
    ProgramId   INT IDENTITY(1,1) PRIMARY KEY,
    ProgramCode VARCHAR(10)   NOT NULL UNIQUE,
    ProgramName NVARCHAR(100) NOT NULL,
    IsActive    BIT           NOT NULL DEFAULT 1,
    CreatedAt   DATETIME2     NOT NULL DEFAULT GETDATE()
);
GO

INSERT INTO Programs (ProgramCode, ProgramName) VALUES
('IF',  'Teknik Informatika'),
('SI',  'Sistem Informasi'),
('TE',  'Teknik Elektro'),
('RKA', 'Rekayasa Kecerdasan Artifisial'),
('RPL', 'Rekayasa Perangkat Lunak');
GO

SELECT * FROM Programs;
GO

-- 3. Membuat tabel Students (Relasi 1:N ke Programs)
CREATE TABLE Students (
    StudentId   INT IDENTITY(1,1) PRIMARY KEY,
    NIM         VARCHAR(20)   NOT NULL UNIQUE,
    Name        NVARCHAR(100) NOT NULL,
    ProgramId   INT           NOT NULL,
    BirthDate   DATE          NOT NULL,
    Address     NVARCHAR(255) NOT NULL,
    PhoneNumber VARCHAR(20)   NOT NULL,
    CreatedAt   DATETIME2     NOT NULL DEFAULT GETDATE(),
    UpdatedAt   DATETIME2     NULL,
    CONSTRAINT FK_Students_Programs FOREIGN KEY (ProgramId)
        REFERENCES Programs(ProgramId)
);
GO

-- 4. Mengisi 5 data mahasiswa awal
INSERT INTO Students (NIM, Name, ProgramId, BirthDate, Address, PhoneNumber) VALUES
('5025231011', 'Raka Pratama Wijaya',  1, '2005-03-14', 'Jl. Keputih Tegal No. 18, Surabaya',     '081234560101'),
('5026231024', 'Nadia Kusuma Putri',   2, '2005-07-22', 'Jl. Gebang Lor No. 42, Surabaya',        '081234560102'),
('5022231035', 'Dimas Aditya Saputra', 3, '2004-11-05', 'Jl. Mulyosari Utara No. 7, Surabaya',    '081234560103'),
('5054231048', 'Aulia Rahmawati',      4, '2005-01-29', 'Jl. Sukolilo Dian Regency Blok B-12',    '081234560104'),
('5053231052', 'Bagas Mahendra Putra', 5, '2005-09-17', 'Jl. Arif Rahman Hakim No. 91, Surabaya', '081234560105');
GO

SELECT * FROM Students;
GO

-- 5. Pengujian Constraint (Jalankan manual satu per satu untuk verifikasi error)
/*
-- Uji Foreign Key (ProgramId 99 tidak ada -> ditolak)
INSERT INTO Students (NIM, Name, ProgramId, BirthDate, Address, PhoneNumber)
VALUES ('5025239991', 'Uji Program Invalid', 99, '2005-05-10', 'Jl. Testing No. 1', '081200000001');

-- Uji UNIQUE NIM (NIM sudah terdaftar -> ditolak)
INSERT INTO Students (NIM, Name, ProgramId, BirthDate, Address, PhoneNumber)
VALUES ('5025231011', 'Uji NIM Duplikat', 1, '2005-06-11', 'Jl. Testing No. 2', '081200000002');
*/

-- 6. Query Latihan (JOIN, WHERE, COUNT, GROUP BY)
SELECT
    s.StudentId,
    s.NIM,
    s.Name,
    p.ProgramCode,
    p.ProgramName,
    s.BirthDate,
    s.Address,
    s.PhoneNumber,
    s.CreatedAt
FROM Students s
INNER JOIN Programs p ON s.ProgramId = p.ProgramId
ORDER BY s.StudentId;
GO

SELECT s.NIM, s.Name, p.ProgramName
FROM Students s
INNER JOIN Programs p ON s.ProgramId = p.ProgramId
WHERE p.ProgramCode = 'IF';
GO

SELECT p.ProgramName, COUNT(s.StudentId) AS JumlahMahasiswa
FROM Programs p
LEFT JOIN Students s ON p.ProgramId = s.ProgramId
GROUP BY p.ProgramName;
GO

-- 7. Menambahkan 3 mahasiswa baru (IF, SI, TE)
INSERT INTO Students (NIM, Name, ProgramId, BirthDate, Address, PhoneNumber) VALUES
('5025231067', 'Fajar Nugroho',        1, '2005-04-08', 'Jl. Klampis Semolo No. 25, Surabaya',   '081234560106'),
('5026231073', 'Salsabila Azzahra',    2, '2005-08-19', 'Jl. Kertajaya Indah No. 63, Surabaya',  '081234560107'),
('5022231089', 'Rizky Ramadhan Hakim', 3, '2004-12-03', 'Jl. Dharmahusada Mas No. 14, Surabaya', '081234560108');
GO

-- 8. Verifikasi hasil akhir JOIN (8 mahasiswa)
SELECT
    s.StudentId,
    s.NIM,
    s.Name,
    p.ProgramName,
    s.BirthDate,
    s.Address,
    s.PhoneNumber,
    s.CreatedAt
FROM Students s
INNER JOIN Programs p ON s.ProgramId = p.ProgramId
ORDER BY s.StudentId;
GO
