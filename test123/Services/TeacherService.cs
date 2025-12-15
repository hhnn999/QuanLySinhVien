using GRPC_NHibernate.Entities;
using NHibernate.Linq;
using Shared;
using Shared.DTOs.RequestModel;
using Shared.DTOs.ResponseModel;
using ISession = NHibernate.ISession;

namespace GRPC_NHibernate.Services;

public class TeacherService : ITeacherService
{
    private readonly ISession _session;

    public TeacherService(ISession session)
    {
        _session = session;
    }

    public async Task CreateTeacherAsync(TeacherRequest request)
    {
        using var tx = _session.BeginTransaction();

        var t = new Teacher
        {
            Code = request.Code,
            FullName = request.FullName,
            BirthDate = request.BirthDate
        };

        await _session.SaveAsync(t);
        await tx.CommitAsync();
    }

    public async Task<List<TeacherResponse>> GetAllTeachersAsync()
    {
        var list = await _session.Query<Teacher>().ToListAsync();

        return list.Select(t => new TeacherResponse
        {
            Id = t.Id,
            Code = t.Code,
            FullName = t.FullName,
            BirthDate = t.BirthDate
        }).ToList();
    }

    public async Task DeleteTeacherAsync(IdRequest request)
    {
        using var tx = _session.BeginTransaction();

        var t = await _session.GetAsync<Teacher>(request.Id)
            ?? throw new Exception("Teacher not found");

        await _session.DeleteAsync(t);
        await tx.CommitAsync();
    }
}
