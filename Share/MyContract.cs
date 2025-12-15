using System.Runtime.Serialization;
using System.ServiceModel;

namespace Shared
{
    [ServiceContract]
    public interface IMyService
    {
        Task<MyServiceResult> DoSomething(MyServiceRequest request);

    }

    [DataContract]
    public class MyServiceResult
    {
        [DataMember(Order = 1)]
        public string NewText { get; set; }

        [DataMember(Order = 2)]
        public int NewValue { get; set; }
    }

    [DataContract]
    public class MyServiceRequest
    {
        [DataMember(Order = 1)]
        public string Text { get; set; }

        [DataMember(Order = 2)]
        public int Value { get; set; }
    }
}
