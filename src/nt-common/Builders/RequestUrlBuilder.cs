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
        public ToastSettings Settings { get; private set; }

        /// <summary>
        /// Gets or sets the BaseUrl for RequestUrlBuilder.
        /// </summary>
        public string BaseUrl { get; private set; }

        /// <summary>
        /// Gets or sets the Version for RequestUrlBuilder.
        /// </summary>
        public string Version { get; private set; }

        /// <summary>
        /// Gets or sets the AppKey for RequestUrlBuilder.
        /// </summary>
        public string AppKey { get; private set; }

        /// <summary>
        /// Gets or sets the Endpoint for RequestUrlBuilder.
        /// </summary>
        public string Endpoint { get; private set; }

        /// <summary>
        /// Gets or sets the Quries Dictionay for RequestUrlBuilder.
        /// </summary>
        public Dictionary<string, string> Queries { get; private set; }

        /// <summary>
        /// Gets or sets the Paths Dictionay for RequestUrlBuilder.
        /// </summary
        public Dictionary<string, string> Paths { get; private set; }

        /// <summary>
        /// Gets or sets the RequestUrl for RequestUrlBuilder.
        /// </summary>
        public string RequestUrl { get; private set; }


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
        public RequestUrlBuilder WithQueries<T>(T queries)
        {
            try
            {
                var serialised = JsonConvert.SerializeObject(queries);
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
        public RequestUrlBuilder WithPaths(string[] paths)
        {
            try
            {
                Paths = Enumerable.Range(0, paths.Length / 2).ToDictionary(i => paths[i * 2], i => paths[i * 2 + 1]);
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

            if (Queries != null)
            {
                foreach (var querie in Queries)
                {
                    var where = querie.Key;

                    where = where.Insert(0, "{");
                    where = where.Insert(where.Length, "}");
                    Endpoint = Endpoint.Replace(where, querie.Value);
                }
            }

            if (Paths != null)
            {
                foreach (var path in Paths)
                {
                    var where = path.Key;
                    where = where.Insert(0, "{");
                    where = where.Insert(where.Length, "}");

                    Endpoint = Endpoint.Replace(where, path.Value);
                }
            }

            return $"{BaseUrl.TrimEnd('/')}/{Endpoint.TrimStart('/')}";
        }
    }
}