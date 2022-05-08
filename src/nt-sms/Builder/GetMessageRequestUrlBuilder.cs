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
    public class GetMessageRequestUrlBuilder : RequestUrlBuilder
    {
        public GetMessageRequestUrlOptions _options = new GetMessageRequestUrlOptions(); 
        private string _endpoint;
        public override GetMessageRequestUrlBuilder WithSettings(ToastSettings<SmsEndpointSettings> setting)
        {
            this._setting = setting;
            this._baseUrl = setting.BaseUrl;
            _options.Version = setting.Version;
            _endpoint = setting.Endpoints.GetMessage;

            return this;
        }
        public override GetMessageRequestUrlBuilder WithHeaders(RequestHeaderModel header)
        {
            _options.AppKey = header.AppKey;

            return this;
        }
        public override GetMessageRequestUrlBuilder WithQueries(HttpRequest req, string requestId)
        {
            _options.RequestId = requestId;
            _options.RecipientSeq = int.TryParse(req.Query["recipientSeq"].ToString(), out int recipientSeqVal) ? recipientSeqVal : 0;

            return this;
        }
        public override string Build()
        {
            _requestUrl = this._setting.Formatter.Format($"{_baseUrl.TrimEnd('/')}/{_endpoint.TrimStart('/')}", _options);

            return _requestUrl;
        }
    }
}
