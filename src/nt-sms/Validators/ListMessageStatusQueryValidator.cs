using System.Threading.Tasks;

using FluentValidation;

using Toast.Common.Exceptions;
using Toast.Sms.Models;

namespace Toast.Sms.Validators
{
    /// <summary>
    /// This represents the extension entity for the request header.
    /// </summary>
    public static class ListMessageStatusRequestQueryValidatorExtension
    {
        /// <summary>
        /// Validates the request query parameters against ListMessageStatus.
        /// </summary>
        /// <param name="headers"><see cref="ListMessageStatusRequestQuries"/> instance.</param>
        /// <returns>Returns the <see cref="ListMessageStatusRequestQuries"/> instance.</returns>
        public static async Task<ListMessageStatusRequestQuries> Validate(this Task<ListMessageStatusRequestQuries> queries, IValidator<ListMessageStatusRequestQuries> validator)
        {
            var instance = await queries.ConfigureAwait(false);

            instance.PageNumber ??= 1;
            instance.PageSize ??= 15;

            var result = validator.Validate(instance);
            if (result.IsValid)
            {
                return instance;
            }

            throw new RequestQueryNotValidException("Not Found") { StatusCode = System.Net.HttpStatusCode.BadRequest };
        }
    }

    /// <summary>
    /// This represents the validator entity for the ListMessageStatus request query parameters.
    /// </summary>
    public class ListMessageStatusRequestQueryValidator : AbstractValidator<ListMessageStatusRequestQuries>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListMessageStatusRequestQueryValidator"/> class.
        /// </summary>
        public ListMessageStatusRequestQueryValidator()
        {
            this.RuleFor(p => p.StartUpdateDate).NotEmpty();
            this.RuleFor(p => p.EndUpdateDate).NotEmpty();
            this.RuleFor(p => p.PageNumber).GreaterThanOrEqualTo(1);
            this.RuleFor(p => p.PageSize).GreaterThanOrEqualTo(1);
        }
    }
}