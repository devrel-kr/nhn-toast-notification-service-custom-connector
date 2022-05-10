using System.Threading.Tasks;

using Toast.Common.Exceptions;
using Toast.Sms.Models;

namespace Toast.Sms.Validators
{
    /// <summary>
    /// This represents the extension entity for the request header.
    /// </summary>
    public static class ListMessageStatusQueryValidator
    {
        /// <summary>
        /// Validates the request query parameters against ListMessageStatus.
        /// </summary>
        /// <param name="headers"><see cref="ListMessageStatusQueryValidator"/> instance.</param>
        /// <returns>Returns the <see cref="ListMessageStatusQueryValidator"/> instance.</returns>
        public static async Task<ListMessageStatusRequestQuries> Validate(this Task<ListMessageStatusRequestQuries> queries)
        {
            var instance = await queries.ConfigureAwait(false);

            if(instance.StartUpdateDate.Length <= 0 || instance.EndUpdateDate.Length <= 0)
            {
                throw new RequestQueryNotValidException("Not Found") { StatusCode = System.Net.HttpStatusCode.BadRequest };
            }

            instance.PageNumber ??= 1;
            instance.PageSize ??= 15;

            return instance;
        }
    }
}