using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using Toast.Common.Models;

namespace Toast.Sms.Models
{
    /// <summary>
    /// This represents the entity for GetMessage response.
    /// </summary>
    public class GetMessageResponse : ResponseModel<ResponseItemBodyModel<GetMessageResponseData>>
    { }

    /// <summary>
    /// This represents the entity for GetMessage response data.
    /// </summary>
    public class GetMessageResponseData
    {
        /// <summary>
        /// Gets or sets the request ID.
        /// </summary>
        public virtual string RequestId { get; set; }

        /// <summary>
        /// Gets or sets the date requested.
        /// </summary>
        public virtual string RequestDate { get; set; }

        /// <summary>
        /// Gets or sets the date resulted.
        /// </summary>
        public virtual string ResultDate { get; set; }

        /// <summary>
        /// Gets or sets the template ID.
        /// </summary>
        public virtual string TemplateId { get; set; }

        /// <summary>
        /// Gets or sets the template ID.
        /// </summary>
        public virtual string TemplateName { get; set; }

        /// <summary>
        /// Gets or sets the category ID.
        /// </summary>
        public virtual string CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the category name.
        /// </summary>
        public virtual string CategoryName { get; set; }

        /// <summary>
        /// Gets or sets the message body.
        /// </summary>
        public virtual string Body { get; set; }

        /// <summary>
        /// Gets or sets the sender's phone number.
        /// </summary>
        [JsonProperty("sendNo")]
        public virtual string SenderNumber { get; set; }

        /// <summary>
        /// Gets or sets the country code.
        /// </summary>
        public virtual string CountryCode { get; set; }

        /// <summary>
        /// Gets or sets the recipient's phone number.
        /// </summary>
        [JsonProperty("recipientNo")]
        public virtual string RecipientNumber { get; set; }

        /// <summary>
        /// Gets or sets the message's status code.
        /// </summary>
        [JsonProperty("msgStatus")]
        public virtual string MessageStatus { get; set; }

        /// <summary>
        /// Gets or sets the message's status name.
        /// </summary>
        [JsonProperty("msgStatusName")]
        public virtual string MessageStatusName { get; set; }

        /// <summary>
        /// Gets or sets the message's result code.
        /// </summary>
        public virtual string ResultCode { get; set; }

        /// <summary>
        /// Gets or sets the message's result name.
        /// </summary>
        public virtual string ResultCodeName { get; set; }

        /// <summary>
        /// Gets or sets the sender's telecom code.
        /// </summary>
        [JsonProperty("telecomCode")]
        public virtual int CarrierCode { get; set; }

        /// <summary>
        /// Gets or sets the sender's telecom name.
        /// </summary>
        [JsonProperty("telecomCodeName")]
        public virtual string CarrierName { get; set; }

        /// <summary>
        /// Gets or sets the request sequence number.
        /// </summary>
        [JsonProperty("recipientSeq")]
        public virtual int RecipientSequence { get; set; }

        /// <summary>
        /// Gets or sets the message type code.
        /// </summary>
        public virtual string SendType { get; set; }

        /// <summary>
        /// Gets or sets the message type name.
        /// </summary>
        public virtual string MessageType { get; set; }

        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        public virtual string UserId { get; set; }

        /// <summary>
        /// Gets or sets whether to advertise.
        /// </summary>
        [JsonProperty("adYn")]
        public virtual string IsAdvertisement { get; set; }

        /// <summary>
        /// Gets or sets the result message.
        /// </summary>
        public virtual string ResultMessage { get; set; }

        /// <summary>
        /// Gets or sets the sender's grouping key.
        /// </summary>
        [JsonProperty("senderGroupingKey")]
        public virtual string SenderGroupKey { get; set; }

        /// <summary>
        /// Gets or sets the recipient's grouping key.
        /// </summary>
        [JsonProperty("recipientGroupingKey")]
        public virtual string RecipientGroupKey { get; set; }
    }
}