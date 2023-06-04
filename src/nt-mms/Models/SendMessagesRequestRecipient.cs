using System.Collections.Generic;

using Newtonsoft.Json;

namespace Toast.Mms.Models
{
    /// <summary>
    /// This represents the entity for SendMessages request recipient.
    /// </summary>
    public class SendMessagesRequestRecipient
    {
        /// <summary>
        /// Gets or sets the recipient's phone number.
        /// </summary>
        [JsonProperty("recipientNo")]
        public virtual string RecipientNumber { get; set; }

        /// <summary>
        /// Gets or sets the country code.
        /// </summary>
        public virtual string CountryCode { get; set; }

        /// <summary>
        /// Gets or sets the recipient's international phone number.
        /// </summary>
        [JsonProperty("internationalRecipientNo")]
        public virtual string InternationalRecipientNumber { get; set; }

        /// <summary>
        /// Gets or sets the collection of template parameters.
        /// </summary>
        [JsonProperty("templateParameter")]
        public virtual Dictionary<string, object> TemplateParameters { get; set; }

        /// <summary>
        /// Gets or sets the recipient's grouping key.
        /// </summary>
        public virtual string RecipientGroupingKey { get; set; }
    }
}