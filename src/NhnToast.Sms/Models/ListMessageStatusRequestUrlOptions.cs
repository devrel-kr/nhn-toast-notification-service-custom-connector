using Toast.Common.Models;

namespace Toast.Sms.Models
{
    /// <summary>
    /// This represents the options for ListMessageStatus request url.
    /// </summary>
    public class ListMessageStatusRequestUrlOptions : RequestUrlOptions
    {
        /// <summary>
        /// Gets or sets the start update date.
        /// </summary>
        public string StartUpdateDate { get; set; }

        /// <summary>
        /// Gets or sets the end update date.
        /// </summary>
        public string EndUpdateDate { get; set; }

        /// <summary>
        /// Gets or sets the message type.
        /// </summary>
        public string MessageType { get; set; }

        /// <summary>
        /// Gets or sets the page number.
        /// </summary>
        public int? PageNum { get; set; }

        /// <summary>
        /// Gets or sets the page size.
        /// </summary>
        public int? PageSize { get; set; }
    }
}