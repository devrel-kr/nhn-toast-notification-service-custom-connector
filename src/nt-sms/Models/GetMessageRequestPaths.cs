using Toast.Common.Models;

namespace Toast.Sms.Models
{
    /// <summary>
    /// This represents the model entity for GetMessage request paths.
    /// </summary>
    public class GetMessageRequestPaths : BaseRequestPaths
    {
        /// <summary>
        /// Gets or sets the request id.
        /// </summary>
        public string RequestId { get; set; }
    }
}
