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
        public virtual string RequestId { get; set; }

        /// <summary>
        /// Gets or sets the request date.
        /// </summary>
        [JsonProperty("requestDate")]
        public virtual string RequestDate { get; set; }

        /// <summary>
        /// Gets or sets the response date.
        /// </summary>
        [JsonProperty("resultDate")]
        public virtual string ResponseDate { get; set; }

        /// <summary>
        /// Gets or sets the template ID.
        /// </summary>
        [JsonProperty("templateId")]
        public virtual string TemplateId { get; set; }

        /// <summary>
        /// Gets or sets the template name.
        /// </summary>
        [JsonProperty("templateName")]
        public virtual string TemplateName { get; set; }

        /// <summary>
        /// Gets or sets the category ID.
        /// </summary>
        [JsonProperty("categoryId")]
        public virtual string CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the category name.
        /// </summary>
        [JsonProperty("categoryName")]
        public virtual string CategoryName { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        [JsonProperty("body")]
        public virtual string Body { get; set; }

        /// <summary>
        /// Gets or sets the send number.
        /// </summary>
        [JsonProperty("sendNo")]
        public virtual string SendNumber { get; set; }

        /// <summary>
        /// Gets or sets the country code.
        /// </summary>
        [JsonProperty("countryCode")]
        public virtual string CountryCode { get; set; }

        /// <summary>
        /// Gets or sets the recipient number.
        /// </summary>
        [JsonProperty("recipientNo")]
        public virtual string RecipientNumber { get; set; }

        /// <summary>
        /// Gets or sets the message status code.
        /// </summary>
        [JsonProperty("msgStatus")]
        public virtual string MsgStatus { get; set; }

        /// <summary>
        /// Gets or sets the message status code name.
        /// </summary>
        [JsonProperty("msgStatusName")]
        public virtual string MsgStatusName { get; set; }

        /// <summary>
        /// Gets or sets the result code.
        /// </summary>
        [JsonProperty("resultCode")]
        public virtual string ResultCode { get; set; }

        /// <summary>
        /// Gets or sets the result code name.
        /// </summary>
        [JsonProperty("resultCodeName")]
        public virtual string ResultCodeName { get; set; }

        /// <summary>
        /// Gets or sets the telecom code.
        /// </summary>
        [JsonProperty("telecomCode")]
        public virtual int TelecomCode { get; set; }

        /// <summary>
        /// Gets or sets the telecom name.
        /// </summary>
        [JsonProperty("telecomCodeName")]
        public virtual string TelecomCodeName { get; set; }

        /// <summary>
        /// Gets or sets the recipient sequence.
        /// </summary>
        [JsonProperty("recipientSeq")]
        public virtual int RecipientSeq { get; set; }

        /// <summary>
        /// Gets or sets the send type.
        /// </summary>
        [JsonProperty("sendType")]
        public virtual string SendType { get; set; }

        /// <summary>
        /// Gets or sets the message type.
        /// </summary>
        [JsonProperty("messageType")]
        public virtual string MessageType { get; set; }

        /// <summary>
        /// Gets or sets the send request ID.
        /// </summary>
        [JsonProperty("userId")]
        public virtual string UserId { get; set; }

        /// <summary>
        /// Gets or sets the whether advertisement.
        /// </summary>
        [JsonProperty("adYn")]
        public virtual string AdYn { get; set; }

        /// <summary>
        /// Gets or sets the result message.
        /// </summary>
        [JsonProperty("resultMessage")]
        public virtual string ResultMessage { get; set; }

        /// <summary>
        /// Gets or sets the sender group key.
        /// </summary>
        [JsonProperty("senderGroupingKey")]
        public virtual string SenderGroupKey { get; set; }

        /// <summary>
        /// Gets or sets the recipient group key.
        /// </summary>
        [JsonProperty("recipientGroupingKey")]
        public virtual string RecipientGroupKey { get; set; }
    }
}