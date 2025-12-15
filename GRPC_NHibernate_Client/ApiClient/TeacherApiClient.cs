using ProtoBuf.Grpc.Client;
using Shared;
using Shared.DTOs.RequestModel;
using Shared.DTOs.ResponseModel;
using Grpc.Net.Client;

namespace GRPC_NHibernate_Client.Services;

public class TeacherApiClient
{
    private readonly ITeacherService _client;

    public TeacherApiClient(GrpcChannel channel)
    {
        _client = channel.CreateGrpcService<ITeacherService>();
    }

    public Task<List<TeacherResponse>> GetAll()
        => _client.GetAllTeachersAsync();

    public Task Create(TeacherRequest req)
        => _client.CreateTeacherAsync(req);

    public Task Delete(IdRequest req)
        => _client.DeleteTeacherAsync(req);
}
