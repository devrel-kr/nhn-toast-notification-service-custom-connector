using Newtonsoft.Json;

namespace Toast.Sms.Models
{
    /// <summary>
    /// This represents the entity for ListMessageStatus request query parameters.
    /// </summary>
    public class ListMessageStatusRequestQuries
    {
        /// <summary>
        /// Gets or sets the start update date.
        /// </summary>
        [JsonProperty("startUpdateDate")]
        public virtual string StartUpdateDate { get; set; }

        /// <summary>
        /// Gets or sets the end update date.
        /// </summary>
        [JsonProperty("endUpdateDate")]
        public virtual string EndUpdateDate { get; set; }

        /// <summary>
        /// Gets or sets the message type.
        /// </summary>
        [JsonProperty("messageType")]
        public virtual string MessageType { get; set; }
        /// <summary>
        /// Gets or sets the page number.
        /// </summary>
        [JsonProperty("pageNum")]
        public virtual int? PageNumber { get; set; }

        /// <summary>
        /// Gets or sets the page size.
        /// </summary>
        public virtual int? PageSize { get; set; }
    }
}