using System.Runtime.Serialization;

namespace Shared.DTOs.ResponseModel
{
    [DataContract]
    public class ClassResponse
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }

        [DataMember(Order = 2)]
        public string Code { get; set; } = string.Empty;

        [DataMember(Order = 3)]
        public string Name { get; set; } = string.Empty;

        [DataMember(Order = 4)]
        public string Subject { get; set; } = string.Empty;

        [DataMember(Order = 5)]
        public TeacherResponse? Teacher { get; set; }
    }
}
