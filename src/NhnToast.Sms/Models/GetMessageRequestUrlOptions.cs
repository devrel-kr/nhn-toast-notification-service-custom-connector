using Toast.Common.Models;

namespace Toast.Sms.Models
{
    /// <summary>
    /// This represents the options for GetMessage request url.
    /// </summary>
    public class GetMessageRequestUrlOptions : RequestUrlOptions
    {
        /// <summary>
        /// Gets or sets the request id.
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// Gets or sets the recipient sequence.
        /// </summary>
        public int? RecipientSeq { get; set; }
    }
}