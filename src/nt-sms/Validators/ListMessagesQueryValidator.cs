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

            if(instance.RequestId.Length > 0 && instance.RequestId.Length <= 25)
            {
                instance.PageNumber ??= 1;
                instance.PageSize ??= 15;
                return instance;
            }

            return instance;

        }
    }
}