using Toast.Common.Models;

namespace Toast.Sms.Models
{
    public class ListMessagesOptions : RequestUrlOptions
    {
        public string RequestId { get; set; }
        public string StartRequestDate { get; set; }
        public string EndRequestDate { get; set; }
        public string StartCreateDate { get; set; }
        public string EndCreateDate { get; set; }
        public string StartResultDate { get; set; }
        public string EndResultDate { get; set; }
        public string SendNo { get; set; }
        public string RecipientNo { get; set; }
        public string TemplateId { get; set; }
        public string MsgStatus { get; set; }
        public string ResultCode { get; set; }
        public string SubResultCode { get; set; }
        public string SenderGroupingKey { get; set; }
        public string RecipientGroupingKey { get; set; }
        public int? PageNum { get; set; }
        public int? PageSize { get; set; }
    }
}