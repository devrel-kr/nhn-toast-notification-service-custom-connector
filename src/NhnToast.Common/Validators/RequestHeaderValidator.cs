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
        /// <remarks>This method will throw <see cref="RequestHeaderNotValidException"/> if the request header is invalid.</remarks>
        public static async Task<RequestHeaderModel> Validate(this Task<RequestHeaderModel> headers)
        {
            var instance = await headers.ConfigureAwait(false);
            if (string.IsNullOrWhiteSpace(instance.AppKey))
            {
                throw new RequestHeaderNotValidException("Not Found") { StatusCode = HttpStatusCode.NotFound };
            }
            if (string.IsNullOrWhiteSpace(instance.SecretKey))
            {
                throw new RequestHeaderNotValidException("Unauthorized") { StatusCode = HttpStatusCode.Unauthorized };
            }

            return instance;
        }
    }
}