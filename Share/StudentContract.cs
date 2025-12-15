using Shared.DTOs.RequestModel;
using Shared.DTOs.ResponseModel;
using System.ServiceModel;

namespace Shared
{
    [ServiceContract]
    public interface IStudentService
    {
        [OperationContract]
        Task AddStudentAsync(StudentRequest request);

        [OperationContract]
        Task UpdateStudentAsync(StudentRequest request);

        [OperationContract]
        Task DeleteStudentAsync(IdRequest request);

        [OperationContract]
        StudentResponse GetStudent(IdRequest request);

        [OperationContract]
        Task<List<StudentResponse>> GetStudentsAsync();
        [OperationContract]
        Task<BasePaginationResponse<StudentResponse>> GetPaginationAsync(StudentPaginationRequest request);
    }
}
