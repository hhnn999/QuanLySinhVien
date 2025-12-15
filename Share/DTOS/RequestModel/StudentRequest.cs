using Common;
using Lucene.Net.Search;
using System.Runtime.Serialization;

namespace Shared.DTOs.RequestModel
{
    [DataContract]
    public class StudentRequest
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }

        [DataMember(Order = 2)]
        public int ClassId { get; set; }

        [DataMember(Order = 3)]
        public string Name { get; set; }

        [DataMember(Order = 4)]
        public DateTime DateOfBirth { get; set; }

        [DataMember(Order = 5)]
        public string Address { get; set; }
    }

    [DataContract]
    public class StudentPaginationRequest
    {
        [DataMember(Order = 1)]
        public Sort? SortByName { get; set; }
        [DataMember(Order = 2)]
        public BasePaginationRequest BasePaginationRequest { get; set; } = new BasePaginationRequest();

    }
}
