using Shared.DTOs.RequestModel;
using Shared.DTOs.ResponseModel;
using System.ServiceModel;

namespace Shared
{
    [ServiceContract]
    public interface ITeacherService
    {
        [OperationContract]
        Task CreateTeacherAsync(TeacherRequest request);

        [OperationContract]
        Task<List<TeacherResponse>> GetAllTeachersAsync();

        [OperationContract]
        Task DeleteTeacherAsync(IdRequest request);
    }
}
