using Newtonsoft.Json;

namespace Toast.Common.Models
{
    /// <summary>
    /// This represents the model entity for request header.
    /// </summary>
    public class RequestHeaderModel
    {
        /// <summary>
        /// Gets or sets the app key header.
        /// </summary>
        [JsonProperty("x-app-key")]
        public virtual string AppKey { get; set; }

        /// <summary>
        /// Gets or sets the secret key header.
        /// </summary>
        [JsonProperty("x-secret-key")]
        public virtual string SecretKey { get; set; }
    }
}