using ProtoBuf.Grpc.Client;
using Shared;
using Shared.DTOs.RequestModel;
using Shared.DTOs.ResponseModel;
using Grpc.Net.Client;

namespace GRPC_NHibernate_Client.Services;

public class ClassRoomApiClient
{
    private readonly IClassService _client;

    public ClassRoomApiClient(GrpcChannel channel)
    {
        _client = channel.CreateGrpcService<IClassService>();
    }

    public Task<List<ClassResponse>> GetAll()
        => _client.GetAllClassRoomsAsync();

    public Task Create(ClassRequest req)
    => _client.CreateClassRoomAsync(req);

    public Task Delete(IdRequest req)
        => _client.DeleteClassRoomAsync(req);
}
