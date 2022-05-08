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
    public class ListMessageStatusRequestUrlBuilder : RequestUrlBuilder
    {
        public ListMessageStatusRequestUrlOptions _options = new ListMessageStatusRequestUrlOptions(); 
        private string _endpoint;
        public override ListMessageStatusRequestUrlBuilder WithSettings(ToastSettings<SmsEndpointSettings> setting)
        {
            this._setting = setting;
            this._baseUrl = setting.BaseUrl;
            _options.Version = setting.Version;
            _endpoint = setting.Endpoints.ListMessageStatus;

            return this;
        }
        public override ListMessageStatusRequestUrlBuilder WithHeaders(RequestHeaderModel header)
        {
            _options.AppKey = header.AppKey;

            return this;
        }
        public override ListMessageStatusRequestUrlBuilder WithQueries(HttpRequest req)
        {
            _options.StartUpdateDate = req.Query["startUpdateDate"].ToString();
            _options.EndUpdateDate = req.Query["endUpdateDate"].ToString();
            _options.MessageType = req.Query["messageType"].ToString();
            _options.PageNum = int.TryParse(req.Query["pageNum"].ToString(), out int pageNumVal) ? pageNumVal : 1;
            _options.PageSize = int.TryParse(req.Query["pageSize"].ToString(), out int pageSizeVal) ? pageSizeVal : 15;

            return this;
        }
        public override string Build()
        {
            _requestUrl = this._setting.Formatter.Format($"{_baseUrl.TrimEnd('/')}/{_endpoint.TrimStart('/')}", _options);
            return _requestUrl;
        }
    }
}
