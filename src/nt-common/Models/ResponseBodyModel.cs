using Newtonsoft.Json;
using System.Collections.Generic;


namespace Toast.Common.Models
{
    /// <summary>
    /// This represents the model entity for get response body.
    /// </summary>
    public abstract class ResponseItemBodyModel<T>
    {
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        [JsonProperty("data")]
        public virtual T Data { get; set; }
    }

    /// <summary>
    /// This represents the model entity for get list response body.
    /// </summary>
    public abstract class ResponseCollectionBodyModel<T>
    {
        /// <summary>
        /// Gets or sets the page number.
        /// </summary>
        [JsonProperty("pageNum")]
        public virtual int PageNumber { get; set; }

        /// <summary>
        /// Gets or sets the page size.
        /// </summary>
        [JsonProperty("pageSize")]
        public virtual int PageSize { get; set; }

        /// <summary>
        /// Gets or sets the total count.
        /// </summary>
        [JsonProperty("totalCount")]
        public virtual int TotalCount { get; set; }

        /// <summary>
        /// Gets or sets the list of data.
        /// </summary>
        [JsonProperty("data")]
        public virtual List<T> Data { get; set; }
    }

}