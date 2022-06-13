using Newtonsoft.Json;

using Toast.Common.Models;

namespace Toast.Sms.Models
{
    /// <summary>
    /// This represents the entity for ListSenders request query parameters.
    /// </summary>
    public class ListSendersRequestQueries : BaseRequestQueries
    {
        /// <summary>
        /// Gets or sets the sender's phone number.
        /// </summary>
        [JsonProperty("sendNo")]
        public virtual string SenderNumber { get; set; }

        /// <summary>
        /// Gets or sets the value indicating whether to use the sender's number or not.
        /// </summary>
        [JsonProperty("useYn")]
        public virtual string UseNumber { get; set; }

        /// <summary>
        /// Gets or sets the value indicating whether the sender's number is blocked or not.
        /// </summary>
        [JsonProperty("blockYn")]
        public virtual string BlockedNumber { get; set; }

        /// <summary>
        /// Gets or sets the current page number.
        /// </summary>
        [JsonProperty("pageNum")]
        public virtual int? PageNumber { get; set; }

        /// <summary>
        /// Gets or sets the page size to return the number of records.
        /// </summary>
        public virtual int? PageSize { get; set; }
    }
}