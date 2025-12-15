using System.Runtime.Serialization;

namespace Shared.DTOs.ResponseModel
{
    [DataContract]
    public class ClassStudentCountResponse
    {
        [DataMember(Order = 1)]
        public string ClassName { get; set; } = string.Empty;

        [DataMember(Order = 2)]
        public int StudentCount { get; set; }
    }
}
