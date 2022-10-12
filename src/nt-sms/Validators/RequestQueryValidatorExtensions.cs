using System.Threading.Tasks;

using FluentValidation;

using Toast.Common.Models;

namespace Toast.Common.Validators
{
    /// <summary>
    /// This represents the extension entity for request queries validator.
    /// </summary>
    public static class RequestQueriesValidatorExtensions
    {
        /// <summary>
        /// Validates request queries.
        /// </summary>
        /// <typeparam name="T">Type of request queries instance.</typeparam>
        /// <param name="queries">Request queries instance.</param>
        /// <param name="validator"><see cref="IValidator{T}"/> instance.</param>
        /// <returns>Returns the validated instance.</returns>
        public static async Task<T> Validate<T>(this Task<T> queries, IValidator<T> validator) where T : BaseRequestQueries
        {
            return await queries.Validate(validator).ConfigureAwait(false);
        }
    }
}