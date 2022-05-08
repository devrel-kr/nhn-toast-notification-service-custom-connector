using Newtonsoft.Json;

namespace Toast.Sms.Models
{
    /// <summary>
    /// This represents the entity for ListMessages request query parameters.
    /// </summary>
    public class ListMessagesRequestQueries
    {
        /// <summary>
        /// Gets or sets the request id.
        /// </summary>
        [JsonProperty("RequestId")]
        public virtual string RequestIden { get; set; }

        /// <summary>
        /// Gets or sets the start request date.
        /// </summary>
        [JsonProperty("StartRequestDate")]
        public virtual string StartReqDate { get; set; }

        /// <summary>
        /// Gets or sets the end request date.
        /// </summary>
        [JsonProperty("EndRequestDate")]
        public virtual string EndReqDate { get; set; }

        /// <summary>
        /// Gets or sets the start create date.
        /// </summary>
        [JsonProperty("StartCreateDate")]
        public virtual string StartCreDate { get; set; }

        /// <summary>
        /// Gets or sets the end create date.
        /// </summary>
        [JsonProperty("EndCreateDate")]
        public virtual string EndCreDate { get; set; }

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