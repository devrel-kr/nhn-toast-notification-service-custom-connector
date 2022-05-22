using Newtonsoft.Json;

using Toast.Common.Models;

namespace Toast.Sms.Models
{
    /// <summary>
    /// This represents the entity for GetMessage request query parameters.
    /// </summary>
    public class GetMessageRequestQueries : BaseRequestQueries
    {
        /// <summary>
        /// Gets or sets the recipient sequence.
        /// </summary>
        [JsonProperty("recipientSeq")]
        public virtual int RecipientSequenceNumber { get; set; }
    }
}