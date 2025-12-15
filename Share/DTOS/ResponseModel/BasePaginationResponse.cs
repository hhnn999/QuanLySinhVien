using System.Runtime.Serialization;
using System.ServiceModel;

namespace Shared.DTOs.ResponseModel
{
    [DataContract]
    public class BasePaginationResponse<T>
    {
        [DataMember(Order = 1)]
        public int PageSize { get; set; }
        [DataMember(Order = 2)]
        public int PageNo { get; set; }
        [DataMember(Order = 3)]
        public int TotalItems { get; set; }
        [DataMember(Order = 4)]
        public int TotalPages { get; set; }
        [DataMember(Order = 5)]
        public List<T> Data { get; set; }
        public BasePaginationResponse(int pageNo, int pageSize, List<T> data, int totalItem)
        {
            PageNo = pageNo;
            PageSize = pageSize;
            TotalItems = totalItem;
            Data = data;
            TotalPages = (totalItem % pageSize) == 0 ? (totalItem / pageSize) : (totalItem / pageSize) + 1;
        }
        public BasePaginationResponse()
        {
            Data = new List<T>();
        }
    }
}
