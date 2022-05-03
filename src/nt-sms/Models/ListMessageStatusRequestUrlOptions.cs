using Toast.Common.Models;

namespace Toast.Sms.Models
{
    /// <summary>
    /// This represents the options for ListMessageStatus request URL.
    /// </summary>
    public class ListMessageStatusRequestUrlOptions : RequestUrlOptions
    {
        /// <summary>
        /// Gets or sets the start update date.
        /// </summary>
        public virtual string StartUpdateDate { get; set; }

        /// <summary>
        /// Gets or sets the end update date.
        /// </summary>
        public virtual string EndUpdateDate { get; set; }

        /// <summary>
        /// Gets or sets the message type.
        /// </summary>
        public virtual string MessageType { get; set; }

        /// <summary>
        /// Gets or sets the page number.
        /// </summary>
        public virtual int? PageNum { get; set; }

        /// <summary>
        /// Gets or sets the page size.
        /// </summary>
        public virtual int? PageSize { get; set; }
    }
}