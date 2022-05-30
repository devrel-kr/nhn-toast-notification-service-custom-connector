using Newtonsoft.Json;

using Toast.Common.Models;

namespace Toast.Sms.Models
{
    /// <summary>
    /// This represents the entity for ListMessageStatus request query parameters.
    /// </summary>
<<<<<<< HEAD:src/nt-sms/Models/ListMessageStatusRequestQuries.cs
    public class ListMessageStatusRequestQueries : BaseRequestQueries
=======
    public class ListMessageStatusRequestQueries
>>>>>>> 032694dea1f196a3e9c6561fa35c011fea1fe4d6:src/nt-sms/Models/ListMessageStatusRequestQueries.cs
    {
        /// <summary>
        /// Gets or sets the start update date.
        /// </summary>
        public virtual string StartUpdateDate { get; set; }

        /// <summary>
        /// Gets or sets the end update date.
        /// </summary>
        public virtual string EndUpdateDate { get; set; }

        /// <summary>
        /// Gets or sets the message type.
        /// </summary>
        public virtual string MessageType { get; set; }

        /// <summary>
        /// Gets or sets the page number.
        /// </summary>
        [JsonProperty("pageNum")]
        public virtual int? PageNumber { get; set; }

        /// <summary>
        /// Gets or sets the page size.
        /// </summary>
        public virtual int? PageSize { get; set; }
    }
}