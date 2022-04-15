public class ListmessageStatusRequestUrlOptions : RequestUrlOptions
{
    public string startUpdateDate { get; set; }
    public string endUpdateDate { get; set; }
    public string messageType { get; set; }
    public int? pageNum { get; set; }
    public int? pageSize { get; set; }
}