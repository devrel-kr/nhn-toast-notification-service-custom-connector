using System.Net;
using System.Threading.Tasks;

using FluentValidation;

using Toast.Common.Exceptions;
using Toast.Sms.Models;

namespace Toast.Sms.Validators
{
    /// <summary>
    /// This represents the extension entity for the request header.
    /// </summary>
    public static class GetMessageRequestQueryValidatorExtension
    {
        /// <summary>
        /// Validates the request query parameters against GetMessage.
        /// </summary>
        /// <param name="headers"><see cref="GetMessageRequestQueries"/> instance.</param>
        /// <returns>Returns the <see cref="GetMessageRequestQueries"/> instance.</returns>
        public static async Task<GetMessageRequestQueries> Validate(this Task<GetMessageRequestQueries> queries, IValidator<GetMessageRequestQueries> validator)
        {
            var instance = await queries.ConfigureAwait(false);

            var result = validator.Validate(instance);
            if (result.IsValid)
            {
                return instance;
            }

            throw new RequestQueryNotValidException("Invalid Query Parameters") { StatusCode = HttpStatusCode.BadRequest };
        }
    }

    /// <summary>
    /// this represents the validator entity for the GetMessage request query parameters.
    /// </summary>
    public class GetMessageRequestQueryValidator : AbstractValidator<GetMessageRequestQueries>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetMessageRequestQueryValidator"/> class.
        /// </summary>
        public GetMessageRequestQueryValidator()
        {
            this.RuleFor(p => p.RecipientSequenceNumber).GreaterThan(0);
        }
    }
}