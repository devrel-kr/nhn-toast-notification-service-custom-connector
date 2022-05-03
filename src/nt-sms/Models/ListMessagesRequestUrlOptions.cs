using Toast.Common.Models;

namespace Toast.Sms.Models
{
    /// <summary>
    /// This represents the options for ListMessages request URL.
    /// </summary>
    public class ListMessagesRequestUrlOptions : RequestUrlOptions
    {
        /// <summary>
        /// Gets or sets the request id.
        /// </summary>
        public virtual string RequestId { get; set; }

        /// <summary>
        /// Gets or sets the start request date.
        /// </summary>
        public virtual string StartRequestDate { get; set; }

        /// <summary>
        /// Gets or sets the end request date.
        /// </summary>
        public virtual string EndRequestDate { get; set; }

        /// <summary>
        /// Gets or sets the start create date.
        /// </summary>
        public virtual string StartCreateDate { get; set; }

        /// <summary>
        /// Gets or sets the end create date.
        /// </summary>
        public virtual string EndCreateDate { get; set; }

        /// <summary>
        /// Gets or sets the start result date.
        /// </summary>
        public virtual string StartResultDate { get; set; }

        /// <summary>
        /// Gets or sets the end result date.
        /// </summary>
        public virtual string EndResultDate { get; set; }

        /// <summary>
        /// Gets or sets the send number.
        /// </summary>
        public virtual string SendNo { get; set; }

        /// <summary>
        /// Gets or sets the recipient number.
        /// </summary>
        public virtual string RecipientNo { get; set; }

        /// <summary>
        /// Gets or sets the template id.
        /// </summary>
        public virtual string TemplateId { get; set; }

        /// <summary>
        /// Gets or sets the message status.
        /// </summary>
        public virtual string MsgStatus { get; set; }

        /// <summary>
        /// Gets or sets the result code.
        /// </summary>
        public virtual string ResultCode { get; set; }

        /// <summary>
        /// Gets or sets the sub result code.
        /// </summary>
        public virtual string SubResultCode { get; set; }

        /// <summary>
        /// Gets or sets the sender grouping key.
        /// </summary>
        public virtual string SenderGroupingKey { get; set; }

        /// <summary>
        /// Gets or sets the recipient grouping key.
        /// </summary>
        public virtual string RecipientGroupingKey { get; set; }

        /// <summary>
        /// Gets or sets the page number.
        /// </summary>
        public virtual int? PageNum { get; set; }

        /// <summary>
        /// Gets or sets the page size.
        /// </summary>
        public virtual int? PageSize { get; set; }
    }
}