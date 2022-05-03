using System.Threading.Tasks;

using Toast.Sms.Verification.Models;

namespace Toast.Sms.Verification.Validators
{
    /// <summary>
    /// This represents the extension entity for the request header.
    /// </summary>
    public static class ListSendersRequestQueryValidator
    {
        /// <summary>
        /// Validates the request query parameters against ListSenders.
        /// </summary>
        /// <param name="headers"><see cref="ListSendersRequestQueries"/> instance.</param>
        /// <returns>Returns the <see cref="ListSendersRequestQueries"/> instance.</returns>
        public static async Task<ListSendersRequestQueries> Validate(this Task<ListSendersRequestQueries> queries)
        {
            var instance = await queries.ConfigureAwait(false);

            instance.PageNumber ??= 1;
            instance.PageSize ??= 15;

            return instance;
        }
    }
}