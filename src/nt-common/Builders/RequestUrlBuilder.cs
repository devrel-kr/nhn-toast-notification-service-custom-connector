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
        /// Gets or sets the Quries for RequestUrlBuilder.
        /// </summary>
        private string Queries;

        /// <summary>
        /// Gets or sets the Paths for RequestUrlBuilder.
        /// </summary
        private string Paths;

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
            var serialised = JsonConvert.SerializeObject(queries, Settings.SerializerSsetting);
            var deserialised = JsonConvert.DeserializeObject<Dictionary<string, string>>(serialised);

            if (deserialised != null) Queries = String.Join("&", deserialised.Select(x => $"{x.Key}={x.Value}"));

            return this;
        }

        /// <summary>
        /// Settings the request URL values in header.
        /// </summary>
        /// <param name="paths"> instance.</param>
        /// <returns>Returns the <see cref="RequestUrlBuilder"/> instance.</returns>
        public RequestUrlBuilder WithPaths<T>(T paths) where T : BaseRequestPaths
        {
            var serialised = JsonConvert.SerializeObject(paths, Settings.SerializerSsetting);
            var deserialised = JsonConvert.DeserializeObject<Dictionary<string, string>>(serialised);

            if (deserialised != null) Paths = String.Join("/", deserialised.Select(x => x.Value));
            
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

            Endpoint = $"{Endpoint.TrimEnd('/')}/{Paths}";
            Endpoint = $"{Endpoint.TrimEnd('/')}?{Queries}";
            Endpoint = Endpoint.TrimEnd('?');

            return $"{BaseUrl.TrimEnd('/')}/{Endpoint.TrimStart('/')}";
        }
    }
}