using System.Runtime.Serialization;

namespace Shared.DTOs.RequestModel
{
    [DataContract]
    public class BasePaginationRequest
    {
        [DataMember(Order = 1)]
        public int Page { get; set; }

        [DataMember(Order = 2)]
        public int PageSize { get; set; }
    }
}
