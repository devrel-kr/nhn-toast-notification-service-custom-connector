using System.Collections.Generic;

using Newtonsoft.Json;

namespace Toast.Sms.Models
{
    /// <summary>
    /// This represents the entity for ListMessages response body.
    /// </summary>
    public class ListMessagesResponseBody
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
        public virtual List<ListMessagesResponseData> datas { get; set; }


    }
}