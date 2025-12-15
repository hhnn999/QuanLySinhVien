using System.Runtime.Serialization;

namespace Shared.DTOs.ResponseModel
{
    [DataContract]
    public class TeacherResponse
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }

        [DataMember(Order = 2)]
        public string Code { get; set; } = string.Empty;

        [DataMember(Order = 3)]
        public string FullName { get; set; } = string.Empty;

        [DataMember(Order = 4)]
        public DateTime? BirthDate { get; set; }
    }
}
