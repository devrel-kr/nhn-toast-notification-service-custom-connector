using Toast.Common.Models;

namespace Toast.Sms.Models
{
    /// <summary>
    /// This represents the options for GetMessage request URL.
    /// </summary>
    public class GetMessageRequestUrlOptions : RequestUrlOptions
    {
        /// <summary>
        /// Gets or sets the request id.
        /// </summary>
        public virtual string RequestId { get; set; }

        /// <summary>
        /// Gets or sets the recipient sequence.
        /// </summary>
        public virtual int? RecipientSeq { get; set; }
    }
}