using System;
using System.Threading.Tasks;

using FluentValidation;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;

using Toast.Sms.Models;

namespace Toast.Sms.Validators
{
    /// <summary>
    /// This represents the extension entity for the request header.
    /// </summary>
    public static class ListSendersRequestQueryValidatorExtension
    {
        /// <summary>
        /// Validates the request query parameters against ListSenders.
        /// </summary>
        /// <param name="headers"><see cref="ListSendersRequestQueries"/> instance.</param>
        /// <returns>Returns the <see cref="ListSendersRequestQueries"/> instance.</returns>
        public static async Task<ListSendersRequestQueries> Validate(this Task<ListSendersRequestQueries> queries, IValidator<ListSendersRequestQueries> validator)
        {
            var instance = await queries.ConfigureAwait(false);

            instance.PageNumber ??= 1;
            instance.PageSize ??= 15;

            var result = validator.Validate(instance);
            if (result.IsValid)
            {
                return instance;
            }

            throw new InvalidOperationException(result.ToString());
        }
    }

    /// <summary>
    /// This represents the validator entity for the ListSenders request query parameters.
    /// </summary>
    public class ListSendersRequestQueryValidator : AbstractValidator<ListSendersRequestQueries>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListSendersRequestQueryValidator"/> class.
        /// </summary>
        public ListSendersRequestQueryValidator()
        {
            this.RuleFor(p => p.UseNumber).Must(BeYOrN)
                                          .When(p => !p.UseNumber.IsNullOrWhiteSpace())
                                          .WithMessage("'useYn' must be 'Y' or 'N'");
            this.RuleFor(p => p.BlockedNumber).Must(BeYOrN)
                                              .When(p => !p.BlockedNumber.IsNullOrWhiteSpace())
                                              .WithMessage("'blockYn' must be 'Y' or 'N'");
            this.RuleFor(p => p.PageNumber).GreaterThanOrEqualTo(1)
                                           .WithMessage("'pageNum' must be greater than or equal to 1");
            this.RuleFor(p => p.PageSize).GreaterThanOrEqualTo(1)
                                         .WithMessage("'pageSize' must be greater than or equal to 1");
        }

        private bool BeYOrN(string value)
        {
            return value == "Y" || value == "N";
        }
    }
}