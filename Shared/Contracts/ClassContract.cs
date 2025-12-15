using Shared.DTOs.RequestModel;
using Shared.DTOs.ResponseModel;
using System.ServiceModel;

namespace Shared
{
    [ServiceContract]
    public interface IClassService
    {
        [OperationContract]
        Task CreateClassRoomAsync(ClassRequest request);

        [OperationContract]
        Task<List<ClassResponse>> GetAllClassRoomsAsync();

        [OperationContract]
        Task DeleteClassRoomAsync(IdRequest request);
    }
}
