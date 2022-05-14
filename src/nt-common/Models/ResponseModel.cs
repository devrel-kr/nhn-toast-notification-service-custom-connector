using Newtonsoft.Json;

namespace Toast.Common.Models
{
    /// <summary>
    /// This represents the model entity for response.
    /// </summary>
    public class ResponseModel<T> 
    {
        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        [JsonProperty("header")]
        public virtual ResponseHeaderModel header { get; set; }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        [JsonProperty("body")]
        public virtual T body { get; set; }

    }

}