using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;

using Toast.Common.Configurations;
using Toast.Common.Models;

namespace Toast.Common.Builders
{
    /// <summary>
    /// This represents the Builder for request URL.
    /// </summary>
    public class RequestUrlBuilder
    {
        private ToastSettings _settings;

        private RequestHeaderModel _headers;

        private string _endpoint;

        private string _queries;

        private Dictionary<string, string> _paths;

        /// <summary>
        /// Settings the request URL values in setting.
        /// </summary>
        /// <param name="settings"> instance.</param>
        /// <param name="endpoint"> instance.</param>
        /// <returns>Returns the <see cref="RequestUrlBuilder"/> instance.</returns>
        public RequestUrlBuilder WithSettings<T>(T settings, string endpoint) where T : ToastSettings
        {
            this._settings = settings;
            this._endpoint = endpoint;

            return this;
        }

        /// <summary>
        /// Settings the request URL values in header.
        /// </summary>
        /// <param name="headers"> <see cref="RequestHeaderModel"/> instance.</param>
        /// <returns>Returns the <see cref="RequestUrlBuilder"/> instance.</returns>
        public RequestUrlBuilder WithHeaders(RequestHeaderModel headers)
        {
            this._headers = headers;

            return this;
        }

        /// <summary>
        /// Settings the request URL values in header.
        /// </summary>
        /// <param name="queries"> instance.</param>
        /// <returns>Returns the <see cref="RequestUrlBuilder"/> instance.</returns>
        public RequestUrlBuilder WithQueries<T>(T queries) where T : BaseRequestQueries
        {
            if (this._settings == null)
            {
                throw new InvalidOperationException("Invalid ToastSettings.");
            }

            var serialised = JsonConvert.SerializeObject(queries, this._settings.JsonFormatter.SerializerSettings);
            var deserialised = JsonConvert.DeserializeObject<Dictionary<string, string>>(serialised);

            this._queries = string.Join("&", deserialised.Select(x => $"{x.Key}={x.Value}"));


            return this;
        }

        /// <summary>
        /// Settings the request URL values in header.
        /// </summary>
        /// <param name="paths"> instance.</param>
        /// <returns>Returns the <see cref="RequestUrlBuilder"/> instance.</returns>
        public RequestUrlBuilder WithPaths<T>(T paths) where T : BaseRequestPaths
        {
            if (this._settings == null)
            {
                throw new InvalidOperationException("Invalid ToastSettings.");
            }

            var serialised = JsonConvert.SerializeObject(paths, this._settings.JsonFormatter.SerializerSettings);
            var deserialised = JsonConvert.DeserializeObject<Dictionary<string, string>>(serialised);

            this._paths = deserialised;


            return this;
        }

        /// <summary>
        /// Settings the request URL values in header.
        /// </summary>
        /// <returns>Returns request URL which is string type instance.</returns>
        public string Build()
        {
            var requestUrl = $"{this._settings.BaseUrl.TrimEnd('/')}/{this._endpoint.TrimStart('/')}";

            requestUrl = requestUrl.Replace("{version}", this._settings.Version);
            requestUrl = requestUrl.Replace("{appKey}", this._headers.AppKey);

            if (this._paths != null)
            {
                foreach (var key in _paths.Keys)
                {
                    requestUrl = requestUrl.Replace($"{{{key}}}", _paths[key]);
                }
            }

            if (!string.IsNullOrWhiteSpace(this._queries))
            {
                requestUrl += $"?{this._queries}";
            }

            return requestUrl;
        }
    }
}