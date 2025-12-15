using System.Runtime.Serialization;

namespace Shared.DTOs.ResponseModel
{
    [DataContract]
    public class BasePaginationResponse<T>
    {
        [DataMember(Order = 1)]
        public int TotalRecords { get; set; }

        [DataMember(Order = 2)]
        public int Page { get; set; }

        [DataMember(Order = 3)]
        public int PageSize { get; set; }

        [DataMember(Order = 4)]
        public List<T> Items { get; set; } = new List<T>();
    }
}
