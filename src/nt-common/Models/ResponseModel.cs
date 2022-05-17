using Newtonsoft.Json;

namespace Toast.Common.Models
{
    /// <summary>
    /// This represents the model entity for response.
    /// </summary>
    public abstract class ResponseModel<T> 
    {
        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        [JsonProperty("header")]
        public virtual ResponseHeaderModel Header { get; set; }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        [JsonProperty("body")]
        public virtual T Body { get; set; }

    }

}