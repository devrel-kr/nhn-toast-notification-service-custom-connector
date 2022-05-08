using Microsoft.AspNetCore.Http;

using Toast.Common.Configurations;
using Toast.Common.Models;
using Toast.Sms.Configurations;
using Toast.Sms.Models;

namespace Toast.Sms.Bulder
{ 
    /// <summary>
    /// This represents the model entity for request url.
    /// </summary>
    public class ListMessagesRequestUrlBuilder : RequestUrlBuilder
    {
        public ListMessagesRequestUrlOptions _options = new ListMessagesRequestUrlOptions(); 
        private string _endpoint;
        public override ListMessagesRequestUrlBuilder WithSettings(ToastSettings<SmsEndpointSettings> setting)
        {
            this._setting = setting;
            this._baseUrl = setting.BaseUrl;
            _options.Version = setting.Version;
            _endpoint = setting.Endpoints.ListMessages;

            return this;
        }
        public override ListMessagesRequestUrlBuilder WithHeaders(RequestHeaderModel header)
        {
            _options.AppKey = header.AppKey;

            return this;
        }
        public override ListMessagesRequestUrlBuilder WithQueries(HttpRequest req)
        {
            _options.RequestId = req.Query["requestId"].ToString();
            _options.StartRequestDate = req.Query["startRequestDate"].ToString();
            _options.EndRequestDate = req.Query["endRequestDate"].ToString();
            _options.StartCreateDate = req.Query["startCreateDate"].ToString();
            _options.EndCreateDate = req.Query["endCreateDate"].ToString();
            _options.StartResultDate = req.Query["startResultDate"].ToString();
            _options.EndResultDate = req.Query["endResultDate"].ToString();
            _options.SendNo = req.Query["sendNo"].ToString();
            _options.RecipientNo = req.Query["recipientNo"].ToString();
            _options.TemplateId = req.Query["templateId"].ToString();
            _options.MsgStatus = req.Query["msgStatus"].ToString();
            _options.ResultCode = req.Query["resultCode"].ToString();
            _options.SubResultCode = req.Query["subResultCode"].ToString();
            _options.SenderGroupingKey = req.Query["senderGroupingKey"].ToString();
            _options.RecipientGroupingKey = req.Query["recipientGroupingKey"].ToString();
            _options.PageNum = int.TryParse(req.Query["pageNum"].ToString(), out int pageNumParse) ? pageNumParse : 1;
            _options.PageSize = int.TryParse(req.Query["pageSize"].ToString(), out int pageSizeParse) ? pageSizeParse : 15;

            return this;
        }
        public override string Build()
        {
            _requestUrl = this._setting.Formatter.Format($"{_baseUrl.TrimEnd('/')}/{_endpoint.TrimStart('/')}", _options);

            return _requestUrl;
        }
    }
}
