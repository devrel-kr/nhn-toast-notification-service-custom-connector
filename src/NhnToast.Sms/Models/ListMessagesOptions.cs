public class ListMessagesOptions : RequestUrlOptions
{
    public string requestId { get; set; }
    public string startRequestDate { get; set; }
    public string endRequestDate { get; set; }
    public string startCreateDate { get; set; }
    public string endCreateDate { get; set; }
    public string startResultDate { get; set; }
    public string endResultDate { get; set; }
    public string sendNo { get; set; }
    public string recipientNo { get; set; }
    public string templateId { get; set; }
    public string msgStatus { get; set; }
    public string resultCode { get; set; }
    public string subResultCode { get; set; }
    public string senderGroupingKey { get; set; }
    public string recipientGroupingKey { get; set; }
    public int? pageNum { get; set; }
    public int? pageSize { get; set; }

}