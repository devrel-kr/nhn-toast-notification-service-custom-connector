using Toast.Common.Models;

namespace Toast.Sms.Models 
{
    /// <summary>
    /// This represents the options for ListMessages request url.
    /// </summary>
    public class ListMessagesOptions : RequestUrlOptions
    {
        /// <summary>
        /// Gets or sets the request id.
        /// </summary>
        public string RequestId { get; set; }
        /// <summary>
        /// Gets or sets the start request date.
        /// </summary>
        public string StartRequestDate { get; set; }
        /// <summary>
        /// Gets or sets the end request date.
        /// </summary>
        public string EndRequestDate { get; set; }
        /// <summary>
        /// Gets or sets the start create date.
        /// </summary>
        public string StartCreateDate { get; set; }
        /// <summary>
        /// Gets or sets the end create date.
        /// </summary>
        public string EndCreateDate { get; set; }
        /// <summary>
        /// Gets or sets the start result date.
        /// </summary>
        public string StartResultDate { get; set; }
        /// <summary>
        /// Gets or sets the end result date.
        /// </summary>
        public string EndResultDate { get; set; }
        /// <summary>
        /// Gets or sets the send number.
        /// </summary>
        public string SendNo { get; set; }
        /// <summary>
        /// Gets or sets the recipient number.
        /// </summary>
        public string RecipientNo { get; set; }
        /// <summary>
        /// Gets or sets the template id.
        /// </summary>
        public string TemplateId { get; set; }
        /// <summary>
        /// Gets or sets the message status.
        /// </summary>
        public string MsgStatus { get; set; }
        /// <summary>
        /// Gets or sets the result code.
        /// </summary>
        public string ResultCode { get; set; }
        /// <summary>
        /// Gets or sets the sub result code.
        /// </summary>
        public string SubResultCode { get; set; }
        /// <summary>
        /// Gets or sets the sender grouping key.
        /// </summary>
        public string SenderGroupingKey { get; set; }
        /// <summary>
        /// Gets or sets the recipient grouping key.
        /// </summary>
        public string RecipientGroupingKey { get; set; }
        /// <summary>
        /// Gets or sets the page number.
        /// </summary>
        public int? PageNum { get; set; }
        /// <summary>
        /// Gets or sets the page size.
        /// </summary>
        public int? PageSize { get; set; }
    }
}
