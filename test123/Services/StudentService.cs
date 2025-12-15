using GRPC_NHibernate.Entities;
using NHibernate.Linq;
using Shared;
using Shared.DTOs.RequestModel;
using Shared.DTOs.ResponseModel;
using ISession = NHibernate.ISession;

namespace GRPC_NHibernate.Services;

public class StudentService : IStudentService
{
    private readonly ISession _session;

    public StudentService(ISession session)
    {
        _session = session;
    }

    // ADD
    public async Task AddStudentAsync(StudentRequest request)
    {
        using var transaction = _session.BeginTransaction();

        var classRoom = await _session.GetAsync<ClassRoom>(request.ClassRoomId)
            ?? throw new Exception("ClassRoom not found");

        var student = new Student
        {
            Code = request.Code,
            FullName = request.FullName,
            Address = request.Address,
            BirthDate = request.BirthDate,
            ClassRoom = classRoom,
        };

        await _session.SaveAsync(student);
        await transaction.CommitAsync();
    }

    // UPDATE
    public async Task UpdateStudentAsync(StudentRequest request)
    {
        using var transaction = _session.BeginTransaction();

        var student = await _session.GetAsync<Student>(request.Id)
            ?? throw new Exception("Student not found");

        var classRoom = await _session.GetAsync<ClassRoom>(request.ClassRoomId)
            ?? throw new Exception("ClassRoom not found");

        student.Code = request.Code;
        student.FullName = request.FullName;
        student.Address = request.Address;
        student.BirthDate = request.BirthDate;
        student.ClassRoom = classRoom;

        await _session.UpdateAsync(student);
        await transaction.CommitAsync();
    }

    // DELETE
    public async Task DeleteStudentAsync(IdRequest request)
    {
        using var transaction = _session.BeginTransaction();

        var student = await _session.GetAsync<Student>(request.Id)
            ?? throw new Exception("Student not found");

        await _session.DeleteAsync(student);
        await transaction.CommitAsync();
    }

    // GET 1 STUDENT
    public StudentResponse GetStudent(IdRequest request)
    {
        var student = _session.Query<Student>()
            .FirstOrDefault(x => x.Id == request.Id)
                ?? throw new Exception("Student not found");

        return MapStudent(student);
    }

    // GET ALL
    public async Task<List<StudentResponse>> GetStudentsAsync()
    {
        var list = await _session.Query<Student>().Fetch(s => s.ClassRoom).ThenFetch(cr => cr.Teacher).ToListAsync();
        return list.Select(MapStudent).ToList();
    }

    // PAGINATION
    public async Task<BasePaginationResponse<StudentResponse>> GetPaginationAsync(StudentPaginationRequest request)
    {
        var query = _session.Query<Student>();

        // Sort
        if (request.SortByName == Common.Sort.Asc)
            query = query.OrderBy(s => s.FullName);
        else if (request.SortByName == Common.Sort.Desc)
            query = query.OrderByDescending(s => s.FullName);

        var total = await query.CountAsync();

        var page = request.BasePaginationRequest.Page;
        var size = request.BasePaginationRequest.PageSize;

        var items = await query
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();

        return new BasePaginationResponse<StudentResponse>
        {
            TotalRecords = total,
            Page = page,
            PageSize = size,
            Items = items.Select(MapStudent).ToList()
        };
    }

    // ==============================
    // MAPPING
    // ==============================

    private StudentResponse MapStudent(Student s) => new StudentResponse
    {
        Id = s.Id,
        Code = s.Code,
        FullName = s.FullName,
        Address = s.Address,
        BirthDate = s.BirthDate ?? DateTime.MinValue,
        ClassRoom = s.ClassRoom != null ? new ClassResponse
        {
            Id = s.ClassRoom.Id,
            Code = s.ClassRoom.Code,
            Name = s.ClassRoom.Name,
            Subject = s.ClassRoom.Subject,
            Teacher = s.ClassRoom.Teacher != null ? new TeacherResponse
            {
                Id = s.ClassRoom.Teacher.Id,
                FullName = s.ClassRoom.Teacher.FullName
            } : null
        } : null
    };

    // ==============================
    // CHART
    // ==============================

    public async Task<StudentCountByClassResponse> GetStudentCountByClassAsync(IdRequest request)
    {
        var counts = await _session.Query<Student>()
            .Where(s => s.ClassRoom != null) // Chỉ sinh viên có lớp
            .GroupBy(s => s.ClassRoom.Id)
            .Select(g => new
            {
                ClassRoomId = g.Key,
                StudentCount = g.Count()
            })
            .ToListAsync();

        var classIds = counts.Select(c => c.ClassRoomId).ToList();
        var classRooms = await _session.Query<ClassRoom>()
            .Where(cr => classIds.Contains(cr.Id))
            .ToListAsync();

        var finalItems = counts.Select(c =>
        {
            var classRoom = classRooms.FirstOrDefault(cr => cr.Id == c.ClassRoomId);
            return new ClassStudentCountResponse
            {
                ClassName = classRoom != null ? $"{classRoom.Code} — {classRoom.Name}" : "N/A",
                StudentCount = c.StudentCount
            };
        }).ToList();

        return new StudentCountByClassResponse
        {
            Items = finalItems
        };
    }
}
