using System.Collections.Generic;

using Newtonsoft.Json;

namespace Toast.Sms.Models
{
    /// <summary>
    /// This represents the entity for SendMessages response body.
    /// </summary>
    public class SendMessagesResponseBody
    {
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        [JsonProperty("data")]
        public virtual SendMessagesResponseData data { get; set; }
        
}