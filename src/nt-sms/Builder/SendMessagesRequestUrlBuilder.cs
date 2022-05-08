using Microsoft.AspNetCore.Http;

using Toast.Common.Configurations;
using Toast.Common.Models;
using Toast.Sms.Configurations;

namespace Toast.Sms.Bulder
{ 
    /// <summary>
    /// This represents the model entity for request url.
    /// </summary>
    public class SendMessagesRequestUrlBuilder : RequestUrlBuilder
    {
        public RequestUrlOptions _options = new RequestUrlOptions(); 
        private string _endpoint;
        public override SendMessagesRequestUrlBuilder WithSettings(ToastSettings<SmsEndpointSettings> setting)
        {
            this._setting = setting;
            this._baseUrl = setting.BaseUrl;
            _options.Version = setting.Version;
            _endpoint = setting.Endpoints.SendMessages;

            return this;
        }
        public override SendMessagesRequestUrlBuilder WithHeaders(RequestHeaderModel header)
        {
            _options.AppKey = header.AppKey;

            return this;
        }
        
        public override string Build()
        {
            _requestUrl = this._setting.Formatter.Format($"{_baseUrl.TrimEnd('/')}/{_endpoint.TrimStart('/')}", _options);

            return _requestUrl;
        }
    }
}
