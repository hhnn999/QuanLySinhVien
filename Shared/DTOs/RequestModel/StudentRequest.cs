using Common;
using System.Runtime.Serialization;

namespace Shared.DTOs.RequestModel
{

    [DataContract]
    public class StudentRequest
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }

        [DataMember(Order = 2)]
        public int ClassRoomId { get; set; }

        [DataMember(Order = 3)]
        public string Code { get; set; } = string.Empty;

        [DataMember(Order = 4)]
        public string FullName { get; set; } = string.Empty;

        [DataMember(Order = 5)]
        public string Address { get; set; } = string.Empty;

        [DataMember(Order = 6)]
        public DateTime? BirthDate { get; set; }
    }

    [DataContract]
    public class StudentPaginationRequest
    {
        // Sắp xếp theo tên
        [DataMember(Order = 1)]
        public Sort? SortByName { get; set; }

        // Page, PageSize
        [DataMember(Order = 2)]
        public BasePaginationRequest BasePaginationRequest { get; set; }
            = new BasePaginationRequest();
    }
}
