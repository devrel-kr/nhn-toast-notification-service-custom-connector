using System.Collections.Generic;

using Newtonsoft.Json;

namespace Toast.Sms.Models
{
    /// <summary>
    /// This represents the entity for ListMessages response data.
    /// </summary>
    public class ListMessagesResponseData
    {
        /// <summary>
        /// Gets or sets the request ID.
        /// </summary>
        [JsonProperty("requestId")]
        public virtual string requestId { get; set; }

        /// <summary>
        /// Gets or sets the request date.
        /// </summary>
        [JsonProperty("requestDate")]
        public virtual string requestDate { get; set; }

        /// <summary>
        /// Gets or sets the response date.
        /// </summary>
        [JsonProperty("resultDate")]
        public virtual string responseDate { get; set; }

        /// <summary>
        /// Gets or sets the template ID.
        /// </summary>
        [JsonProperty("templateId")]
        public virtual string templateId { get; set; }

        /// <summary>
        /// Gets or sets the template name.
        /// </summary>
        [JsonProperty("templateName")]
        public virtual string templateName { get; set; }

        /// <summary>
        /// Gets or sets the category ID.
        /// </summary>
        [JsonProperty("categoryId")]
        public virtual string categoryId { get; set; }

        /// <summary>
        /// Gets or sets the category name.
        /// </summary>
        [JsonProperty("categoryName")]
        public virtual string categoryName { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        [JsonProperty("body")]
        public virtual string body { get; set; }

        /// <summary>
        /// Gets or sets the send number.
        /// </summary>
        [JsonProperty("sendNo")]
        public virtual string sendNumber { get; set; }

        /// <summary>
        /// Gets or sets the country code.
        /// </summary>
        [JsonProperty("countryCode")]
        public virtual string countryCode { get; set; }

        /// <summary>
        /// Gets or sets the recipient number.
        /// </summary>
        [JsonProperty("recipientNo")]
        public virtual string recipientNumber { get; set; }

        /// <summary>
        /// Gets or sets the message status code.
        /// </summary>
        [JsonProperty("msgStatus")]
        public virtual string msgStatus { get; set; }

        /// <summary>
        /// Gets or sets the message status code name.
        /// </summary>
        [JsonProperty("msgStatusName")]
        public virtual string msgStatusName { get; set; }

        /// <summary>
        /// Gets or sets the result code.
        /// </summary>
        [JsonProperty("resultCode")]
        public virtual string resultCode { get; set; }

        /// <summary>
        /// Gets or sets the result code name.
        /// </summary>
        [JsonProperty("resultCodeName")]
        public virtual string resultCodeName { get; set; }

        /// <summary>
        /// Gets or sets the telecom code.
        /// </summary>
        [JsonProperty("telecomCode")]
        public virtual int telecomCode { get; set; }

        /// <summary>
        /// Gets or sets the telecom name.
        /// </summary>
        [JsonProperty("telecomCodeName")]
        public virtual string telecomCodeName { get; set; }

        /// <summary>
        /// Gets or sets the recipient sequence.
        /// </summary>
        [JsonProperty("recipientSeq")]
        public virtual int recipientSeq { get; set; }

        /// <summary>
        /// Gets or sets the send type.
        /// </summary>
        [JsonProperty("sendType")]
        public virtual string sendType { get; set; }

        /// <summary>
        /// Gets or sets the message type.
        /// </summary>
        [JsonProperty("messageType")]
        public virtual string messageType { get; set; }

        /// <summary>
        /// Gets or sets the send request ID.
        /// </summary>
        [JsonProperty("userId")]
        public virtual string userId { get; set; }

        /// <summary>
        /// Gets or sets the whether advertisement.
        /// </summary>
        [JsonProperty("adYn")]
        public virtual string adYn { get; set; }

        /// <summary>
        /// Gets or sets the result message.
        /// </summary>
        [JsonProperty("resultMessage")]
        public virtual string resultMessage { get; set; }

        /// <summary>
        /// Gets or sets the sender group key.
        /// </summary>
        [JsonProperty("senderGroupingKey")]
        public virtual string senderGroupKey { get; set; }

        /// <summary>
        /// Gets or sets the recipient group key.
        /// </summary>
        [JsonProperty("recipientGroupingKey")]
        public virtual string recipientGroupKey { get; set; }
    }
}