using Microsoft.Data.SqlClient;
using StudentRegistrationApp.Models;

namespace StudentRegistrationApp.Data;

public sealed class MahasiswaRepository
{
    private const string ConnectionString =
        "Server=localhost\\SQLEXPRESS;Database=StudentRegistrationDB;Trusted_Connection=True;TrustServerCertificate=True;";

    public List<Mahasiswa> GetAll()
    {
        var students = new List<Mahasiswa>();

        const string query = """
            SELECT
                s.StudentId,
                s.NIM,
                s.Name,
                s.ProgramId,
                p.ProgramName,
                s.BirthDate,
                s.Address,
                s.PhoneNumber,
                s.CreatedAt
            FROM Students s
            INNER JOIN Programs p ON s.ProgramId = p.ProgramId
            ORDER BY s.StudentId;
            """;

        using var connection = new SqlConnection(ConnectionString);
        using var command = new SqlCommand(query, connection);

        connection.Open();
        using SqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            students.Add(new Mahasiswa
            {
                StudentId = reader.GetInt32(0),
                Nim = reader.GetString(1),
                Nama = reader.GetString(2),
                ProgramId = reader.GetInt32(3),
                ProgramName = reader.GetString(4),
                BirthDate = reader.GetDateTime(5),
                Alamat = reader.GetString(6),
                NoTelepon = reader.GetString(7),
                Dibuat = reader.GetDateTime(8)
            });
        }

        return students;
    }

    public int GetTotal()
    {
        const string query = "SELECT COUNT(*) FROM Students;";

        using var connection = new SqlConnection(ConnectionString);
        using var command = new SqlCommand(query, connection);

        connection.Open();
        return Convert.ToInt32(command.ExecuteScalar());
    }
}
