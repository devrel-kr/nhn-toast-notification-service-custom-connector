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
        /// <summary>
        /// Gets or sets the Settings for RequestUrlBuilder.
        /// </summary>
        private ToastSettings _settings { get; set; }

        /// <summary>
        /// Gets or sets the BaseUrl for RequestUrlBuilder.
        /// </summary>
        private string _baseUrl { get; set; }

        /// <summary>
        /// Gets or sets the Version for RequestUrlBuilder.
        /// </summary>
        private string _version { get; set; }

        /// <summary>
        /// Gets or sets the AppKey for RequestUrlBuilder.
        /// </summary>
        private string _appKey { get; set; }

        /// <summary>
        /// Gets or sets the Endpoint for RequestUrlBuilder.
        /// </summary>
        private string _endpoint { get; set; }

        /// <summary>
        /// Gets or sets the Quries for RequestUrlBuilder.
        /// </summary>
        private string _queries { get; set; }

        /// <summary>
        /// Gets or sets the Paths for RequestUrlBuilder.
        /// </summary
        private Dictionary<string, string> _paths { get; set; }

        /// <summary>
        /// Settings the request URL values in setting.
        /// </summary>
        /// <param name="settings"> instance.</param>
        /// <param name="endpoint"> instance.</param>
        /// <returns>Returns the <see cref="RequestUrlBuilder"/> instance.</returns>
        public RequestUrlBuilder WithSettings<T>(T settings, string endpoint) where T : ToastSettings
        {
            if (settings != null)
            {
                _settings = settings;
                _baseUrl = settings.BaseUrl;
                _version = settings.Version;
                _endpoint = endpoint;

            }

            return this;
        }

        /// <summary>
        /// Settings the request URL values in header.
        /// </summary>
        /// <param name="headers"> <see cref="RequestHeaderModel"/> instance.</param>
        /// <returns>Returns the <see cref="RequestUrlBuilder"/> instance.</returns>
        public RequestUrlBuilder WithHeaders(RequestHeaderModel headers)
        {
            if (headers != null) _appKey = headers.AppKey;
            
            return this;
        }

        /// <summary>
        /// Settings the request URL values in header.
        /// </summary>
        /// <param name="queries"> instance.</param>
        /// <returns>Returns the <see cref="RequestUrlBuilder"/> instance.</returns>
        public RequestUrlBuilder WithQueries<T>(T queries) where T : BaseRequestQueries
        {
            var serialised = JsonConvert.SerializeObject(queries, _settings?.SerializerSetting);
            var deserialised = JsonConvert.DeserializeObject<Dictionary<string, string>>(serialised);

            if (deserialised != null && deserialised.Count() != 0) _queries = String.Join("&", deserialised.Select(x => $"{x.Key}={x.Value}"));

            return this;
        }

        /// <summary>
        /// Settings the request URL values in header.
        /// </summary>
        /// <param name="paths"> instance.</param>
        /// <returns>Returns the <see cref="RequestUrlBuilder"/> instance.</returns>
        public RequestUrlBuilder WithPaths<T>(T paths) where T : BaseRequestPaths
        {
            var serialised = JsonConvert.SerializeObject(paths, _settings?.SerializerSetting);
            var deserialised = JsonConvert.DeserializeObject<Dictionary<string, string>>(serialised);
            if (deserialised != null && deserialised.Count() != 0) _paths = deserialised;
            return this;
        }

        /// <summary>
        /// Settings the request URL values in header.
        /// </summary>
        /// <returns>Returns request URL which is string type instance.</returns>
        public string Build()
        {
            if (_baseUrl == null || _endpoint == null) return $"{_baseUrl?.TrimEnd('/')}/{_endpoint?.TrimStart('/')}";
            
            _endpoint = _endpoint.Replace("{version}", _version);
            _endpoint = _endpoint.Replace("{appKey}", _appKey);

            if (_paths != null)
            {
                foreach (var i in _paths.Keys)
                {
                    _endpoint = _endpoint.Replace($"{{{i}}}", _paths[i]);
                }
            }

            _endpoint = $"{_endpoint.TrimEnd('/')}?{_queries}";
            _endpoint = _endpoint.TrimEnd('?');

            return $"{_baseUrl.TrimEnd('/')}/{_endpoint.TrimStart('/')}";
        }
    }
}