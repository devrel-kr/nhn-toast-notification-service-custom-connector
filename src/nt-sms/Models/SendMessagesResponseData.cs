using System.Collections.Generic;

using Newtonsoft.Json;

namespace Toast.Sms.Models
{
    /// <summary>
    /// This represents the entity for SendMessages response data.
    /// </summary>
    public class SendMessagesResponseData
    {
        /// <summary>
        /// Gets or sets the request ID.
        /// </summary>
        [JsonProperty("requestId")]
        public virtual string RequestId { get; set; }

        /// <summary>
        /// Gets or sets the request status code.
        /// </summary>
        /// 
        [JsonProperty("statusCode")]
        public virtual string StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the sender group key.
        /// </summary>
        [JsonProperty("senderGroupingKey")]
        public virtual string SenderGroupingKey { get; set; }

        /// <summary>
        /// Gets or sets the list of send results.
        /// </summary>
        [JsonProperty("sendResultList")]
        public virtual List<SendMessagesResponseResult> SendResults { get; set; }

    }
}