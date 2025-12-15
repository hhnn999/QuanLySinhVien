using QuanLySinhVien.Repositories;
using System.Data;
using System.Data.SqlClient;

public class StudentRepositoryAdo : IStudentRepository
{
    private readonly string _connString;
    public StudentRepositoryAdo(string connString) => _connString = connString;

    public async Task AddAsync(Student s)
    {
        using var conn = new SqlConnection(_connString);
        var cmd = new SqlCommand("INSERT INTO Student (Code, FullName, BirthDate, Address, ClassId) VALUES (@Code,@FullName,@BirthDate,@Address,@ClassId)", conn);
        cmd.Parameters.AddWithValue("@Code", s.Code);
        cmd.Parameters.AddWithValue("@FullName", s.FullName);
        cmd.Parameters.AddWithValue("@BirthDate", (object?)s.BirthDate ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Address", s.Address ?? "");
        cmd.Parameters.AddWithValue("@ClassId", (object?)s.ClassRoom?.ClassId ?? DBNull.Value);
        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task DeleteAsync(int id)
    {
        using var conn = new SqlConnection(_connString);
        var cmd = new SqlCommand("DELETE FROM Student WHERE StudentId=@id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<IEnumerable<Student>> GetAllAsync()
    {
        var list = new List<Student>();
        using var conn = new SqlConnection(_connString);
        var cmd = new SqlCommand(@"SELECT s.StudentId,s.Code,s.FullName,s.BirthDate,s.Address,
                                        c.ClassId,c.Code as ClassCode,c.Name as ClassName,c.Subject,
                                        t.TeacherId,t.Code as TeacherCode,t.FullName as TeacherName,t.BirthDate as TeacherBD
                                  FROM Student s
                                  LEFT JOIN ClassRoom c ON s.ClassId = c.ClassId
                                  LEFT JOIN Teacher t ON c.TeacherId = t.TeacherId", conn);
        await conn.OpenAsync();
        using var rdr = await cmd.ExecuteReaderAsync();
        while (await rdr.ReadAsync())
        {
            var student = new Student
            {
                StudentId = rdr.GetInt32(rdr.GetOrdinal("StudentId")),
                Code = rdr.GetString(rdr.GetOrdinal("Code")),
                FullName = rdr.GetString(rdr.GetOrdinal("FullName")),
                BirthDate = rdr.IsDBNull(rdr.GetOrdinal("BirthDate")) ? null : rdr.GetDateTime(rdr.GetOrdinal("BirthDate")),
                Address = rdr.IsDBNull(rdr.GetOrdinal("Address")) ? "" : rdr.GetString(rdr.GetOrdinal("Address")),
                ClassRoom = null
            };
            if (!rdr.IsDBNull(rdr.GetOrdinal("ClassId")))
            {
                student.ClassRoom = new ClassRoom
                {
                    ClassId = rdr.GetInt32(rdr.GetOrdinal("ClassId")),
                    Code = rdr.IsDBNull(rdr.GetOrdinal("ClassCode")) ? "" : rdr.GetString(rdr.GetOrdinal("ClassCode")),
                    Name = rdr.IsDBNull(rdr.GetOrdinal("ClassName")) ? "" : rdr.GetString(rdr.GetOrdinal("ClassName")),
                    Subject = rdr.IsDBNull(rdr.GetOrdinal("Subject")) ? "" : rdr.GetString(rdr.GetOrdinal("Subject")),
                    Teacher = null
                };
                if (!rdr.IsDBNull(rdr.GetOrdinal("TeacherId")))
                {
                    student.ClassRoom.Teacher = new Teacher
                    {
                        TeacherId = rdr.GetInt32(rdr.GetOrdinal("TeacherId")),
                        Code = rdr.IsDBNull(rdr.GetOrdinal("TeacherCode")) ? "" : rdr.GetString(rdr.GetOrdinal("TeacherCode")),
                        FullName = rdr.IsDBNull(rdr.GetOrdinal("TeacherName")) ? "" : rdr.GetString(rdr.GetOrdinal("TeacherName")),
                        BirthDate = rdr.IsDBNull(rdr.GetOrdinal("TeacherBD")) ? null : rdr.GetDateTime(rdr.GetOrdinal("TeacherBD"))
                    };
                }
            }
            list.Add(student);
        }
        return list;
    }

    public async Task<Student?> GetByIdAsync(int id)
    {
        var all = await GetAllAsync();
        return all.FirstOrDefault(s => s.StudentId == id);
    }

    public async Task<Student?> GetByCodeAsync(string code)
    {
        var all = await GetAllAsync();
        return all.FirstOrDefault(s => s.Code.Equals(code, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<IEnumerable<Student>> GetSortedByNameAsync()
    {
        var all = (await GetAllAsync()).OrderBy(s => s.FullName).ToList();
        return all;
    }

    public async Task<IEnumerable<Student>> SearchByNameAsync(string namePart)
    {
        return (await GetAllAsync()).Where(s => s.FullName.IndexOf(namePart, StringComparison.OrdinalIgnoreCase) >= 0);
    }

    public async Task UpdateAsync(Student s)
    {
        using var conn = new SqlConnection(_connString);
        var cmd = new SqlCommand("UPDATE Student SET Code=@Code,FullName=@FullName,BirthDate=@BirthDate,Address=@Address,ClassId=@ClassId WHERE StudentId=@id", conn);
        cmd.Parameters.AddWithValue("@Code", s.Code);
        cmd.Parameters.AddWithValue("@FullName", s.FullName);
        cmd.Parameters.AddWithValue("@BirthDate", (object?)s.BirthDate ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Address", s.Address ?? "");
        cmd.Parameters.AddWithValue("@ClassId", (object?)s.ClassRoom?.ClassId ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@id", s.StudentId);
        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }
}
