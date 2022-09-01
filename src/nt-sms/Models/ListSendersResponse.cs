using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using Toast.Common.Models;

namespace Toast.Sms.Models
{
    /// <summary>
    /// This represents the entity for ListSenders response.
    /// </summary>
    public class ListSendersResponse : ResponseModel<ResponseCollectionBodyModel<ListSendersResponseData>>
    { }

    /// <summary>
    /// This represents the entity for ListSenders response data.
    /// </summary>
    public class ListSendersResponseData
    {
        /// <summary>
        /// Gets or sets the service ID.
        /// </summary>
        public virtual int ServiceId { get; set; }

        /// <summary>
        /// Gets or sets the sender's phone number.
        /// </summary>
        [JsonProperty("sendNo")]
        public virtual string SenderNumber { get; set; }

        /// <summary>
        /// Gets or sets the value indicating whether to use the sender's number or not.
        /// </summary>
        [JsonProperty("useYn")]
        public virtual string UseNumber { get; set; }

        /// <summary>
        /// Gets or sets the value indicating whether the sender's number is blocked or not.
        /// </summary>
        [JsonProperty("blockYn")]
        public virtual string BlockedNumber { get; set; }

        /// <summary>
        /// Gets or sets the reason the number is blocked.
        /// </summary>
        [JsonProperty("blockReason")]
        public virtual string BlockedReason { get; set; }

        /// <summary>
        /// Gets or sets the date created.
        /// </summary>
        public virtual string CreateDate { get; set; }

        /// <summary>
        /// Gets or sets the user ID who created the record.
        /// </summary>
        public virtual string CreateUser { get; set; }

        /// <summary>
        /// Gets or sets the date updated.
        /// </summary>
        public virtual string UpdateDate { get; set; }

        /// <summary>
        /// Gets or sets the user ID who updated the record.
        /// </summary>
        public virtual string UpdateUser { get; set; }
    }
}