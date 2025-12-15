using Shared.DTOs.ResponseModel;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Shared
{
    [ServiceContract]
    public interface IClassService
    {
        public Task<List<ClassResponse>> GetClassesAsync();
    }
}
