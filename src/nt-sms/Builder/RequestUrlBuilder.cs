using Microsoft.AspNetCore.Http;

using Toast.Common.Configurations;
using Toast.Common.Models;
using Toast.Sms.Configurations;

namespace Toast.Sms.Bulder
{
    /// <summary>
    /// This represents the model entity for request url.
    /// </summary>
    public class RequestUrlBuilder
    {
        protected ToastSettings<SmsEndpointSettings> _setting;
        protected HttpRequest _req;
        protected string _baseUrl;
        protected string _version;
        protected string _requestUrl;

        /// <summary>
        /// 
        /// </summary>
        public virtual RequestUrlBuilder WithSettings(ToastSettings<SmsEndpointSettings> setting)
        {
            return this;
        }

        public virtual RequestUrlBuilder WithHeaders(RequestHeaderModel header)
        {
            return this;
        }
        public virtual RequestUrlBuilder WithQueries(HttpRequest req)
        {
            return this;
        }
        public virtual RequestUrlBuilder WithQueries(HttpRequest req, string requestId)
        {
            return this;
        }
        public virtual string Build( )
        {
            return _requestUrl;
        }
    }
}
