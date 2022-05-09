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
        [JsonProperty("requestId")]
        public virtual string RequestIden { get; set; }

        /// <summary>
        /// Gets or sets the start request date.
        /// </summary>
        [JsonProperty("startRequestDate")]
        public virtual string StartReqDate { get; set; }

        /// <summary>
        /// Gets or sets the end request date.
        /// </summary>
        [JsonProperty("endRequestDate")]
        public virtual string EndReqDate { get; set; }

        /// <summary>
        /// Gets or sets the start create date.
        /// </summary>
        [JsonProperty("startCreateDate")]
        public virtual string StartCreDate { get; set; }

        /// <summary>
        /// Gets or sets the end create date.
        /// </summary>
        [JsonProperty("endCreateDate")]
        public virtual string EndCreDate { get; set; }

        /// <summary>
        /// Gets or sets the start result date.
        /// </summary>
        [JsonProperty("startResultDate")]
        public virtual string StartResultDate { get; set; }

        /// <summary>
        /// Gets or sets the end result date.
        /// </summary>
        [JsonProperty("endResultDate")]
        public virtual string EndResultDate { get; set; }

        /// <summary>
        /// Gets or sets the send number.
        /// </summary>
        [JsonProperty("sendNo")]
        public virtual string SendNumber { get; set; }

        /// <summary>
        /// Gets or sets the recipient number.
        /// </summary>
        [JsonProperty("recipientNo")]
        public virtual string RecipientNumber { get; set; }

        /// <summary>
        /// Gets or sets the template id.
        /// </summary>
        [JsonProperty("templateId")]
        public virtual string TemplateId { get; set; }

        /// <summary>
        /// Gets or sets the message status.
        /// </summary>
        [JsonProperty("msgStatus")]
        public virtual string MessageStatus { get; set; }

        /// <summary>
        /// Gets or sets the reusult code.
        /// </summary>
        [JsonProperty("resultCode")]
        public virtual string ResultCode { get; set; }

        /// <summary>
        /// Gets or sets the sub result code.
        /// </summary>
        [JsonProperty("subResultCode")]
        public virtual string SubResultCode { get; set; }

        /// <summary>
        /// Gets or sets the sender grouping key.
        /// </summary>
        [JsonProperty("senderGroupingKey")]
        public virtual string SenderGroupingKey { get; set; }

        /// <summary>
        /// Gets or sets the recipient grouping key.
        /// </summary>
        [JsonProperty("recipientGroupingKey")]
        public virtual string RecipientGroupingKey { get; set; }

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