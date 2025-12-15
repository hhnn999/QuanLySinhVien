using System.Runtime.Serialization;

namespace Shared.DTOs.RequestModel
{
    [DataContract]
    public class BasePaginationRequest
    {
        [DataMember(Order = 1)]
        public int PageNo { get; set; } = 1;
        [DataMember(Order = 2)]
        public int PageSize { get; set; } = 10;
        [DataMember(Order = 3)]
        public string? Keyword { get; set; } = string.Empty;
    }
}
