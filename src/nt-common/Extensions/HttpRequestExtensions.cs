using System;
using System.Linq;
using System.Text;

using Microsoft.AspNetCore.Http;

using Toast.Common.Models;

namespace Toast.Common.Extensions
{
    /// <summary>
    /// This represents the extension entity for <see cref="HttpRequest"/>.
    /// </summary>
    public static class HttpRequestExtensions
    {
        /// <summary>
        /// Convert the HTTP request header in <see cref="HttpRequest"/> to <see cref="RequestHeaderModel"/>.
        /// </summary>
        /// <typeparam name="T">Type of HTTP request header model.</typeparam>
        /// <param name="req"><see cref="HttpRequest"/> instance.</param>
        /// <param name="useBasicAuthHeader">Value indicating whether to use the basic auth header or not.</param>
        /// <returns>Returns <see cref="RequestHeaderModel"/> instance.</returns>
        public static T To<T>(this HttpRequest req, bool useBasicAuthHeader) where T : RequestHeaderModel
        {
            var values = req.Headers["Authorization"]
                            .ToString()
                            .Replace("Basic", string.Empty)
                            .Trim()
                            .Decode()
                            .Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries)
                            .ToArray();

            var headers = new RequestHeaderModel() { AppKey = values.First(), SecretKey = values.Last() };

            return (T)headers;
        }

        private static string Decode(this string base64EncodedValue)
        {
            var bytes = Convert.FromBase64String(base64EncodedValue);
            var decoded = Encoding.UTF8.GetString(bytes);

            return decoded;
        }
    }
}