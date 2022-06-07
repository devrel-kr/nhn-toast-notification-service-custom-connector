using System.Net;
using System.Threading.Tasks;

using Toast.Common.Exceptions;
using Toast.Common.Models;

namespace Toast.Common.Validators
{
    /// <summary>
    /// This represents the extension entity for the request header.
    /// </summary>
    public static class RequestHeaderValidator
    {
        /// <summary>
        /// Validates the request header.
        /// </summary>
        /// <param name="headers"><see cref="RequestHeaderModel"/> instance.</param>
        /// <returns>Returns the <see cref="RequestHeaderModel"/> instance.</returns>
        /// <remarks>This method will throw <see cref="RequestHeaderNotValidException"/> if the request header is invalid.</remarks>
        public static async Task<RequestHeaderModel> Validate(this Task<RequestHeaderModel> headers)
        {
            var instance = await headers.ConfigureAwait(false);

            return Validate(instance);
        }

        /// <summary>
        /// Validates the request header.
        /// </summary>
        /// <param name="headers"><see cref="RequestHeaderModel"/> instance.</param>
        /// <returns>Returns the <see cref="RequestHeaderModel"/> instance.</returns>
        /// <remarks>This method will throw <see cref="RequestHeaderNotValidException"/> if the request header is invalid.</remarks>
        public static RequestHeaderModel Validate(this RequestHeaderModel headers)
        {
            if (string.IsNullOrWhiteSpace(headers.AppKey))
            {
                throw new RequestHeaderNotValidException("Not Found") { StatusCode = HttpStatusCode.NotFound };
            }
            if (string.IsNullOrWhiteSpace(headers.SecretKey))
            {
                throw new RequestHeaderNotValidException("Unauthorized") { StatusCode = HttpStatusCode.Unauthorized };
            }

            return headers;
        }
    }
}