using System.Runtime.Serialization;

namespace Shared.DTOs.ResponseModel
{
    [DataContract]
    public class StudentCountByClassResponse
    {
        [DataMember(Order = 1)]
        public List<ClassStudentCountResponse> Items { get; set; } = new List<ClassStudentCountResponse>();
    }
}