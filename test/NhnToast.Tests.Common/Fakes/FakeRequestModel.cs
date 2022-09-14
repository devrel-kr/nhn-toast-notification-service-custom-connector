using Toast.Common.Models;

namespace Toast.Tests.Common.Fakes
{
    public class FakeRequestModel : BaseRequestPayload
    {
        public virtual string FakeProperty1 { get; set; }
    }
}