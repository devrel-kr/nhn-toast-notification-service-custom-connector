using System.Collections.Generic;

using Newtonsoft.Json;

namespace Toast.Sms.Models
{
    /// <summary>
    /// This represents the entity for SendMessages response send result.
    /// </summary>
    public class SendMessagesResponseResult
    {
        /// <summary>
        /// Gets or sets the recipient number.
        /// </summary>
        [JsonProperty("recipientNo")]
        public virtual string recipientNumber { get; set; }

        /// <summary>
        /// Gets or sets the result code.
        /// </summary>
        [JsonProperty("resultCode")]
        public virtual int resultCode { get; set; }

        /// <summary>
        /// Gets or sets the result message.
        /// </summary>
        [JsonProperty("resultMessage")]
        public virtual string resultMessage { get; set; }

        /// <summary>
        /// Gets or sets the recipient sequence.
        /// </summary>
        [JsonProperty("recipientSeq")]
        public virtual int recipientSequence { get; set; }

        /// <summary>
        /// Gets or sets the recipient group key.
        /// </summary>
        [JsonProperty("recipientGroupingKey")]
        public virtual string recipientGroupKey { get; set; }


    }
}