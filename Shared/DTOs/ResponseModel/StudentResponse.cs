using System.Runtime.Serialization;

namespace Shared.DTOs.ResponseModel
{
    [DataContract]
    public class StudentResponse
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }

        [DataMember(Order = 2)]
        public string Code { get; set; } = string.Empty;

        [DataMember(Order = 3)]
        public string FullName { get; set; } = string.Empty;

        [DataMember(Order = 4)]
        public string Address { get; set; } = string.Empty;

        [DataMember(Order = 5)]
        public DateTime? BirthDate { get; set; }

        [DataMember(Order = 6)]
        public ClassResponse? ClassRoom { get; set; }
    }
}
