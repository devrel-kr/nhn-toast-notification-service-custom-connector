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
        private ToastSettings Settings;

        /// <summary>
        /// Gets or sets the BaseUrl for RequestUrlBuilder.
        /// </summary>
        private string BaseUrl;

        /// <summary>
        /// Gets or sets the Version for RequestUrlBuilder.
        /// </summary>
        private string Version;

        /// <summary>
        /// Gets or sets the AppKey for RequestUrlBuilder.
        /// </summary>
        private string AppKey;

        /// <summary>
        /// Gets or sets the Endpoint for RequestUrlBuilder.
        /// </summary>
        private string Endpoint;

        /// <summary>
        /// Gets or sets the Quries Dictionay for RequestUrlBuilder.
        /// </summary>
        private Dictionary<string, string> Queries;

        /// <summary>
        /// Gets or sets the Paths Dictionay for RequestUrlBuilder.
        /// </summary
        private Dictionary<string, string> Paths;

        /// <summary>
        /// Gets or sets the RequestUrl for RequestUrlBuilder.
        /// </summary>
        private string RequestUrl;

        /// <summary>
        /// Settings the request URL values in setting.
        /// </summary>
        /// <param name="settings"> instance.</param>
        /// <param name="endpoint"> instance.</param>
        /// <returns>Returns the <see cref="RequestUrlBuilder"/> instance.</returns>
        public RequestUrlBuilder WithSettings<T> (T settings, string endpoint) where T : ToastSettings
        {
            Settings = settings;
            BaseUrl = settings.BaseUrl;
            Version = settings.Version;
            Endpoint = endpoint;

            return this;
        }

        /// <summary>
        /// Settings the request URL values in header.
        /// </summary>
        /// <param name="headers"> <see cref="RequestHeaderModel"/> instance.</param>
        /// <returns>Returns the <see cref="RequestUrlBuilder"/> instance.</returns>
        public RequestUrlBuilder WithHeaders(RequestHeaderModel headers)
        {
            AppKey = headers.AppKey;
            
            return this;
        }

        /// <summary>
        /// Settings the request URL values in header.
        /// </summary>
        /// <param name="queries"> instance.</param>
        /// <returns>Returns the <see cref="RequestUrlBuilder"/> instance.</returns>
        public RequestUrlBuilder WithQueries<T>(T queries) where T : BaseRequestQueries
        {
            try
            {
                var serialised = JsonConvert.SerializeObject(queries, Settings.SerializerSsetting);
                Queries = JsonConvert.DeserializeObject<Dictionary<string, string>>(serialised);
            }
            catch(Exception ex)
            {
                Queries = null;
            }

            return this;
        }

        /// <summary>
        /// Settings the request URL values in header.
        /// </summary>
        /// <param name="paths"> instance.</param>
        /// <returns>Returns the <see cref="RequestUrlBuilder"/> instance.</returns>
        public RequestUrlBuilder WithPaths<T>(T paths) where T : BaseRequestPaths
        {
            try
            {
                var serialised = JsonConvert.SerializeObject(paths, Settings.SerializerSsetting);
                Paths = JsonConvert.DeserializeObject<Dictionary<string, string>>(serialised);
            }
            catch(Exception ex)
            {
                Paths = null;
            }
            
            
            return this;
        }

        /// <summary>
        /// Settings the request URL values in header.
        /// </summary>
        /// <returns>Returns request URL which is string type instance.</returns>
        public string Build()
        {
            if (BaseUrl == null) return $"{BaseUrl}/{Endpoint.TrimStart('/')}";
            else if (Endpoint == null) return $"{BaseUrl.TrimEnd('/')}/{Endpoint}";

            Endpoint = Endpoint.Replace("{version}", Version);
            Endpoint = Endpoint.Replace("{appKey}", AppKey);

            Endpoint += String.Join("&", Queries.Select(x => x.Key + "=" + x.Value).ToArray());

            Endpoint += String.Join("&", Paths.Select(x => x.Key + "=" + x.Value).ToArray());

            return $"{BaseUrl.TrimEnd('/')}/{Endpoint.TrimStart('/')}";
        }
    }
}