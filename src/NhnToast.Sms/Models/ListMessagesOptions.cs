using Toast.Common.Models;

namespace Toast.Sms.Models {
    /// <summary>
    /// This represents the options for ListMessages request url.
    /// </summary>
    public class ListMessagesOptions : RequestUrlOptions
    {
        /// <summary>
        /// Gets or sets the request id.
        /// </summary>
        public string requestId { get; set; }
        /// <summary>
        /// Gets or sets the start request date.
        /// </summary>
        public string startRequestDate { get; set; }
        /// <summary>
        /// Gets or sets the end request date.
        /// </summary>
        public string endRequestDate { get; set; }
        /// <summary>
        /// Gets or sets the start create date.
        /// </summary>
        public string startCreateDate { get; set; }
        /// <summary>
        /// Gets or sets the end create date.
        /// </summary>
        public string endCreateDate { get; set; }
        /// <summary>
        /// Gets or sets the start result date.
        /// </summary>
        public string startResultDate { get; set; }
        /// <summary>
        /// Gets or sets the end result date.
        /// </summary>
        public string endResultDate { get; set; }
        /// <summary>
        /// Gets or sets the send number.
        /// </summary>
        public string sendNo { get; set; }
        /// <summary>
        /// Gets or sets the recipient number.
        /// </summary>
        public string recipientNo { get; set; }
        /// <summary>
        /// Gets or sets the template id.
        /// </summary>
        public string templateId { get; set; }
        /// <summary>
        /// Gets or sets the message status.
        /// </summary>
        public string msgStatus { get; set; }
        /// <summary>
        /// Gets or sets the result code.
        /// </summary>
        public string resultCode { get; set; }
        /// <summary>
        /// Gets or sets the sub result code.
        /// </summary>
        public string subResultCode { get; set; }
        /// <summary>
        /// Gets or sets the sender grouping key.
        /// </summary>
        public string senderGroupingKey { get; set; }
        /// <summary>
        /// Gets or sets the recipient grouping key.
        /// </summary>
        public string recipientGroupingKey { get; set; }
        /// <summary>
        /// Gets or sets the page number.
        /// </summary>
        public int? pageNum { get; set; }
        /// <summary>
        /// Gets or sets the page size.
        /// </summary>
        public int? pageSize { get; set; }
    }
}
