using System.Runtime.Serialization;

namespace Shared.DTOs.ResponseModel
{
    [DataContract]
    public class StudentResponse
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }

        [DataMember(Order = 2)]
        public string Name { get; set; }

        [DataMember(Order = 3)]
        public DateTime DateOfBirth { get; set; }

        [DataMember(Order = 4)]
        public string Address { get; set; }

        [DataMember(Order = 5)]
        public ClassResponse Class { get; set; }

        [DataMember(Order = 6)]
        public string TeacherName { get; set; }
    }
}
