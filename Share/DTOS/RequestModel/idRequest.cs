using System.Runtime.Serialization;

namespace Shared.DTOs.RequestModel
{
    [DataContract]
    public class IdRequest
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }
    }
}
