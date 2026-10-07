using System.Collections.ObjectModel;
using StudentRegistrationApp.Models;

namespace StudentRegistrationApp.Data;

public sealed class MahasiswaRepository
{
    private readonly ObservableCollection<Mahasiswa> students = [];

    public IReadOnlyList<Mahasiswa> GetAll()
    {
        return students;
    }

    public bool ExistsNim(string nim, Mahasiswa? excludedStudent = null)
    {
        return students.Any(student => student != excludedStudent
            && student.Nim.Equals(nim, StringComparison.OrdinalIgnoreCase));
    }

    public void Add(Mahasiswa student)
    {
        students.Add(student);
    }

    public void Update(Mahasiswa currentStudent, Mahasiswa updatedStudent)
    {
        int index = students.IndexOf(currentStudent);
        if (index >= 0)
        {
            students[index] = updatedStudent;
        }
    }

    public void Remove(Mahasiswa student)
    {
        students.Remove(student);
    }
}