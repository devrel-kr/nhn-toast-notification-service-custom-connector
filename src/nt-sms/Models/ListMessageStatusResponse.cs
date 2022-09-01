using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using Toast.Common.Models;

namespace Toast.Sms.Models
{
    /// <summary>
    /// This represents the entity for ListMessageStatus response.
    /// </summary>
    public class ListMessageStatusResponse : ResponseModel<ResponseCollectionBodyModel<ListMessageStatusResponseData>>
    { }

    /// <summary>
    /// This represents the entity for ListMessageStatus response data.
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
        public virtual int RecipientSequence { get; set; }

        /// <summary>
        /// Gets or sets the message's result code.
        /// </summary>
        public virtual string ResultCode { get; set; }

        /// <summary>
        /// Gets or sets the message's result name.
        /// </summary>
        [JsonProperty("resultCodeName")]
        public virtual string ResultName { get; set; }

        /// <summary>
        /// Gets or sets the date requested.
        /// </summary>
        public virtual string RequestDate { get; set; }

        /// <summary>
        /// Gets or sets the date resulted.
        /// </summary>
        public virtual string ResultDate { get; set; }

        /// <summary>
        /// Gets or sets the date updated.
        /// </summary>
        public virtual string UpdateDate { get; set; }

        /// <summary>
        /// Gets or sets the sender's telecom code.
        /// </summary>
        [JsonProperty("telecomCode")]
        public virtual string CarrierCode { get; set; }

        /// <summary>
        /// Gets or sets the sender's telecom name.
        /// </summary>
        [JsonProperty("telecomCodeName")]
        public virtual string CarrierName { get; set; }

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