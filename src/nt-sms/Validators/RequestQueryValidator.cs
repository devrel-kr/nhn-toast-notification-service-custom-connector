using System.Threading.Tasks;

using FluentValidation;

using Toast.Common.Models;

namespace Toast.Common.Validators
{
    public static class RequestQueryValidator
    {
        public static async Task<T> Validate<T>(this Task<T> queries, IValidator<T> validator) where T : BaseRequestQueries
        {
            return await queries.Validate(validator).ConfigureAwait(false);
        }
    }
}