using Newtonsoft.Json;
using System.Collections.Generic;


namespace Toast.Common.Models
{
    /// <summary>
    /// This represents the model entity for get list response body.
    /// </summary>
    public class ResponseBodyListModel<T>
    {
        /// <summary>
        /// Gets or sets the page number.
        /// </summary>
        [JsonProperty("pageNum")]
        public virtual int pageNumber { get; set; }

        /// <summary>
        /// Gets or sets the page size.
        /// </summary>
        [JsonProperty("pageSize")]
        public virtual int pageSize { get; set; }

        /// <summary>
        /// Gets or sets the total count.
        /// </summary>
        [JsonProperty("totalCount")]
        public virtual int totalCount { get; set; }

        /// <summary>
        /// Gets or sets the list of data.
        /// </summary>
        [JsonProperty("data")]
        public virtual List<T> datas { get; set; }
    }

}