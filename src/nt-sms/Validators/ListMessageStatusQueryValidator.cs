using System.Threading.Tasks;

using Toast.Sms.Models;

namespace Toast.Sms.Validators
{
    /// <summary>
    /// This represents the extension entity for the request header.
    /// </summary>
    public static class ListMessageStatusQueryValidator
    {
        /// <summary>
        /// Validates the request query parameters against GetMessage.
        /// </summary>
        /// <param name="headers"><see cref="ListMessageStatusQueryValidator"/> instance.</param>
        /// <returns>Returns the <see cref="ListMessagesQueryValidator"/> instance.</returns>
        public static async Task<ListMessageStatusRequestQuries> Validate(this Task<ListMessageStatusRequestQuries> queries)
        {
            var instance = await queries.ConfigureAwait(false);

            instance.PageNumber ??= 1;
            instance.PageSize ??= 15;

            return instance;
        }
    }
}