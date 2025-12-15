using ProtoBuf.Grpc.Client;
using Shared;
using Shared.DTOs.RequestModel;
using Shared.DTOs.ResponseModel;
using Grpc.Net.Client;

namespace GRPC_NHibernate_Client.Services;

public class StudentApiClient
{
    private readonly IStudentService _client;

    public StudentApiClient(GrpcChannel channel)
    {
        _client = channel.CreateGrpcService<IStudentService>();
    }

    public Task<List<StudentResponse>> GetStudents()
        => _client.GetStudentsAsync();

    public StudentResponse GetStudent(int id)
        => _client.GetStudent(new IdRequest { Id = id });

    public Task AddStudent(StudentRequest req)
        => _client.AddStudentAsync(req);

    public Task UpdateStudent(StudentRequest req)
        => _client.UpdateStudentAsync(req);

    public Task DeleteStudent(IdRequest req)
        => _client.DeleteStudentAsync(req);

    public Task<StudentCountByClassResponse> GetStudentCountByClassAsync(IdRequest req)
        => _client.GetStudentCountByClassAsync(req);
}
