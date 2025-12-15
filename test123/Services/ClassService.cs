using GRPC_NHibernate.Entities;
using NHibernate;
using NHibernate.Linq;
using Shared;
using Shared.DTOs.RequestModel;
using Shared.DTOs.ResponseModel;
using ISession = NHibernate.ISession;


namespace GRPC_NHibernate.Services;

public class ClassService : IClassService
{
    private readonly ISession _session;

    public ClassService(ISession session)
    {
        _session = session;
    }

    public async Task CreateClassRoomAsync(ClassRequest request)
    {
        using var tx = _session.BeginTransaction();

        Teacher? teacher = null;
        if (request.TeacherId != 0)
        {
            teacher = await _session.GetAsync<Teacher>(request.TeacherId)
                ?? throw new Exception("Teacher not found");
        }

        var c = new ClassRoom
        {
            Code = request.Code,
            Name = request.Name,
            Subject = request.Subject,
            Teacher = teacher
        };

        await _session.SaveAsync(c);
        await tx.CommitAsync();
    }

    public async Task<List<ClassResponse>> GetAllClassRoomsAsync()
    {
        var list = await _session.Query<ClassRoom>().ToListAsync();

        return list.Select(c => new ClassResponse
        {
            Id = c.Id,
            Code = c.Code,
            Name = c.Name,
            Subject = c.Subject,
            Teacher = c.Teacher != null ? new TeacherResponse
            {
                Id = c.Teacher.Id,
                Code = c.Teacher.Code,
                FullName = c.Teacher.FullName,
                BirthDate = c.Teacher.BirthDate
            } : null
        }).ToList();
    }

    public async Task DeleteClassRoomAsync(IdRequest request)
    {
        using var tx = _session.BeginTransaction();

        var c = await _session.GetAsync<ClassRoom>(request.Id)
            ?? throw new Exception("ClassRoom not found");

        foreach (var student in c.Students.ToList())
        {
            student.ClassRoom = null;
            await _session.UpdateAsync(student);
        }

        await _session.DeleteAsync(c);
        await tx.CommitAsync();
    }
}
