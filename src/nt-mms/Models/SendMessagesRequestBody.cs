using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Toast.Mms.Models
{
    /// <summary>
    /// This represents the entity for SendMessages request body.
    /// </summary>
    public class SendMessagesRequestBody
    {
        /// <summary>
        /// Gets or sets the template ID.
        /// </summary>
        public virtual string TemplateId { get; set; }

        /// <summary>
        /// Gets or sets the message title.
        /// </summary>
        public virtual string Title { get; set; }

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
        /// Gets or sets the date requested.
        /// </summary>
        public virtual string RequestDate { get; set; }

        /// <summary>
        /// Gets or sets the sender's grouping key.
        /// </summary>
        public virtual string SenderGroupingKey { get; set; }

        /// <summary>
        /// Gets or sets the list of recipients.
        /// </summary>
        [JsonProperty("recipientList")]
        public virtual List<SendMessagesRequestRecipient> Recipients { get; set; }

        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        public virtual string UserId { get; set; }

        /// <summary>
        /// Gets or sets the stats ID.
        /// </summary>
        public virtual string StatsId { get; set; }

        /// <summary>
        /// Gets or sets the origin code for identifiation.
        /// </summary>
        public virtual string OriginCode { get; set; }
    }
}