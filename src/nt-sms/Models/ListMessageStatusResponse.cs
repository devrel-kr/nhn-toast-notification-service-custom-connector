using Toast.Common.Models;
using Newtonsoft.Json;

namespace Toast.Sms.Models
{
    /// <summary>
    /// This represents the entity for ListMessageStatus response.
    /// </summary>
    public class ListMessageStatusResponse : ResponseModel<ListMessageStatusResponseBody>
    { }

    /// <summary>
    /// This represents the entity for ListMessageStatus response body.
    /// </summary>
    public class ListMessageStatusResponseBody : ResponseCollectionBodyModel<ListMessageStatusResponseData>
    { }

    /// <summary>
    /// This represents the entity for ListMessageStatus response body's data.
    /// </summary>
    public class ListMessageStatusResponseData
    {
        /// <summary>
        /// Gets or sets the message type name.
        /// </summary>
        public virtual string MessageType { get; set; }

        /// <summary>
        /// Gets or sets the request ID.
        /// </summary>
        public virtual string RequestId { get; set; }

        /// <summary>
        /// Gets or sets the request sequence number.
        /// </summary>
        [JsonProperty("recipientSeq")]
        public virtual string RecipientSequence { get; set; }

        /// <summary>
        /// Gets or sets the message's result code.
        /// </summary>
        public virtual string ResultCode { get; set; }

        /// <summary>
        /// Gets or sets the message's result name.
        /// </summary>
        public virtual string ResultCodeName { get; set; }

        /// <summary>
        /// Gets or sets the date requested.
        /// </summary>
        public virtual string RequestDate { get; set; }

        /// <summary>
        /// Gets or sets the date updated.
        /// </summary>
        public virtual string UpdateDate { get; set; }

        /// <summary>
        /// Gets or sets the sender's telecom code.
        /// </summary>
        public virtual string TelecomCode { get; set; }

        /// <summary>
        /// Gets or sets the sender's telecom name.
        /// </summary>
        public virtual string TelecomCodeName { get; set; }

        /// <summary>
        /// Gets or sets the sender's grouping key.
        /// </summary>
        public virtual string SenderGroupingKey { get; set; }

        /// <summary>
        /// Gets or sets the recipient's grouping key.
        /// </summary>
        public virtual string RecipientGroupingKey { get; set; }
    }
}