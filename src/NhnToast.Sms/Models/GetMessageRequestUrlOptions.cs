using Toast.Common.Models;

namespace Toast.Sms.Models {
    public class GetMessageRequestUrlOptions : RequestUrlOptions
    {
        public string RequestId { get; set; }
        public int? RecipientSeq { get; set; }
    }
}
