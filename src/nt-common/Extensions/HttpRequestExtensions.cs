using System;
using System.Linq;
using System.Text;

using Microsoft.AspNetCore.Http;

using Toast.Common.Models;

namespace Toast.Common.Extensions
{
    public static class HttpRequestExtensions
    {
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