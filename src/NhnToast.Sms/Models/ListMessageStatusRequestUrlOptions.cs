using Toast.Common.Models;

namespace Toast.Sms.Models {
    public class ListMessageStatusRequestUrlOptions : RequestUrlOptions
    {
        public string StartUpdateDate { get; set; }
        public string EndUpdateDate { get; set; }
        public string MessageType { get; set; }
        public int? PageNum { get; set; }
        public int? PageSize { get; set; }
    }
}