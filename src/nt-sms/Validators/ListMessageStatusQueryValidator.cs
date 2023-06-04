using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

using FluentValidation;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;

using Toast.Common.Exceptions;
using Toast.Common.Validators;
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
        public static async Task<ListMessageStatusRequestQueries> Validate(this Task<ListMessageStatusRequestQueries> queries, IValidator<ListMessageStatusRequestQueries> validator)
        {
            var instance = await queries.ConfigureAwait(false);

            instance.PageNumber ??= 1;
            instance.PageSize ??= 15;

            var result = validator.Validate(instance);
            if (result.IsValid)
            {
                return instance;
            }

            throw new RequestQueryNotValidException("Invalid Query Parameter") { StatusCode = HttpStatusCode.BadRequest };
        }
    }

    /// <summary>
    /// This represents the validator entity for the ListMessageStatus request query parameters.
    /// </summary>
    public class ListMessageStatusRequestQueryValidator : AbstractValidator<ListMessageStatusRequestQueries>
    {
        private readonly IRegexDateTimeWrapper _regex;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListMessageStatusRequestQueryValidator"/> class.
        /// </summary>
        public ListMessageStatusRequestQueryValidator(IRegexDateTimeWrapper regex)
        {
            this._regex = regex.ThrowIfNullOrDefault();

            this.RuleFor(p => p.StartUpdateDate).Must(IsValidDateFormat).NotEmpty();
            this.RuleFor(p => p.EndUpdateDate).Must(IsValidDateFormat).NotEmpty().GreaterThan(q => q.StartUpdateDate);
            When(p => p.MessageType != null, () =>
            {
                this.RuleFor(p => p.MessageType).Must(p => MsgType.Contains(p));
            });
            this.RuleFor(p => p.PageNumber).GreaterThan(0);
            this.RuleFor(p => p.PageSize).GreaterThan(0);
        }

        private bool IsValidDateFormat(string date)
        {
            return this._regex.IsMatch(date);
        }

        private List<string> MsgType = new List<string>() { "SMS", "LMS", "MMS", "AUTH" };
    }
}