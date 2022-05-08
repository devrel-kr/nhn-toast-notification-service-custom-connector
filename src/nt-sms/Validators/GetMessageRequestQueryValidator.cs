using System.Threading.Tasks;

using Toast.Sms.Models;

namespace Toast.Sms.Validators
{
    /// <summary>
    /// This represents the extension entity for the request header.
    /// </summary>
    public static class GetMessageRequestQueryValidator
    {
        /// <summary>
        /// Validates the request query parameters against GetMessage.
        /// </summary>
        /// <param name="headers"><see cref="GetMessageRequestQueries"/> instance.</param>
        /// <returns>Returns the <see cref="GetMessageRequestQueries"/> instance.</returns>
        public static async Task<GetMessageRequestQueries> Validate(this Task<GetMessageRequestQueries> queries)
        {
            var instance = await queries.ConfigureAwait(false);

            instance.RecipientSequence ??= 0;

            return instance;
        }
    }
}