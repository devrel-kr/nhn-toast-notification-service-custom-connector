using Toast.Common.Models;

namespace Toast.Sms.Models
{
    /// <summary>
    /// This represents the options for ListSenders request URL.
    /// </summary>
    public class ListSendersRequestUrlOptions : RequestUrlOptions
    {
        /// <summary>
        /// Gets or sets the sender's phone number.
        /// </summary>
        public virtual string SendNo { get; set; }

        /// <summary>
        /// Gets or sets the value indicating whether to use the sender's number or not.
        /// </summary>
        public virtual string UseYn { get; set; }

        /// <summary>
        /// Gets or sets the value indicating whether the sender's number is blocked or not.
        /// </summary>
        public virtual string BlockYn { get; set; }

        /// <summary>
        /// Gets or sets the current page number.
        /// </summary>
        public virtual int? PageNum { get; set; }

        /// <summary>
        /// Gets or sets the page size to return the number of records.
        /// </summary>
        public virtual int? PageSize { get; set; }
    }
}