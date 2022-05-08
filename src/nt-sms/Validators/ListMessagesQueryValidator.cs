using System.Threading.Tasks;

using Toast.Sms.Models;

namespace Toast.Sms.Validators
{
    /// <summary>
    /// This represents the extension entity for the request header.
    /// </summary>
    public static class ListMessagesQueryValidator
    {
        /// <summary>
        /// Validates the request query parameters against ListMessages.
        /// </summary>
        /// <param name="headers"><see cref="ListMessagesRequestQueries"/> instance.</param>
        /// <returns>Returns the <see cref="ListMessagesRequestQueries"/> instance.</returns>
        public static async Task<ListMessagesRequestQueries> Validate(this Task<ListMessagesRequestQueries> queries)
        {
            var instance = await queries.ConfigureAwait(false);

            instance.PageNumber ??= 1;
            instance.PageSi ??= 15;

            return instance;
        }
    }
}