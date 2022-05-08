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
        [JsonProperty("StartUpdateDate")]
        public virtual string StartUpdateDay { get; set; }

        /// <summary>
        /// Gets or sets the end update date.
        /// </summary>
        [JsonProperty("EndUpdateDate")]
        public virtual string EndUpdateDay { get; set; }

        /// <summary>
        /// Gets or sets the page number.
        /// </summary>
        [JsonProperty("PageNum")]
        public virtual int? PageNumber { get; set; }

        /// <summary>
        /// Gets or sets the page size.
        /// </summary>
        public virtual int? PageSize { get; set; }
    }
}