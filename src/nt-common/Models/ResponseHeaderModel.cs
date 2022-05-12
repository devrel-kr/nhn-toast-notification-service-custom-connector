using Newtonsoft.Json;

namespace Toast.Common.Models
{
    /// <summary>
    /// This represents the model entity for response header.
    /// </summary>
    public class ResponseHeaderModel
    {
        /// <summary>
        /// Gets or sets whether success header.
        /// </summary>
        [JsonProperty("isSuccessful")]
        public virtual bool isSuccessful { get; set; }

        /// <summary>
        /// Gets or sets the failure code header.
        /// </summary>
        [JsonProperty("resultCode")]
        public virtual int resultCode { get; set; }

        /// <summary>
        /// Gets or sets the failure message header.
        /// </summary>
        [JsonProperty("resultMessage")]
        public virtual string resultMessage { get; set; }
    }

}