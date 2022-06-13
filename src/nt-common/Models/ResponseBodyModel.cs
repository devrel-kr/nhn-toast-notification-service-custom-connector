using System.Collections.Generic;

using Newtonsoft.Json;

namespace Toast.Common.Models
{
    /// <summary>
    /// This represents the model entity for get response body.
    /// </summary>
    public class ResponseItemBodyModel<T>
    {
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        public virtual T Data { get; set; }
    }

    /// <summary>
    /// This represents the model entity for get list response body.
    /// </summary>
    public class ResponseCollectionBodyModel<T>
    {
        /// <summary>
        /// Gets or sets the page number.
        /// </summary>
        [JsonProperty("pageNum")]
        public virtual int? PageNumber { get; set; }

        /// <summary>
        /// Gets or sets the page size.
        /// </summary>
        public virtual int? PageSize { get; set; }

        /// <summary>
        /// Gets or sets the total count.
        /// </summary>
        public virtual int? TotalCount { get; set; }

        /// <summary>
        /// Gets or sets the list of data.
        /// </summary>
        public virtual List<T> Data { get; set; }
    }
}