using Newtonsoft.Json;
using System.Collections.Generic;


namespace Toast.Common.Models
{
    /// <summary>
    /// This represents the model entity for get response body.
    /// </summary>
    public class ResponseBodyModel<T>
    {
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        [JsonProperty("data")]
        public virtual T data { get; set; }
    }

}