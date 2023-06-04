using System.Net;
using System.Threading.Tasks;

using FluentValidation;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;

using Toast.Common.Exceptions;
using Toast.Common.Validators;
using Toast.Mms.Models;

namespace Toast.Mms.Validators
{
    /// <summary>
    /// This represents the extension entity for the request header.
    /// </summary>
    public static class SendMessagesRequestBodyValidatorExtension
    {
        /// <summary>
        /// Validates the request body parameters against SendMessages.
        /// </summary>
        /// <param name="headers"><see cref="SendMessagesRequestBody"/> instance.</param>
        /// <returns>Returns the <see cref="SendMessagesRequestBody"/> instance.</returns>
        public static async Task<SendMessagesRequestBody> Validate(this Task<SendMessagesRequestBody> queries, IValidator<SendMessagesRequestBody> validator)
        {
            var instance = await queries.ConfigureAwait(false);

            var result = validator.Validate(instance);
            if (result.IsValid)
            {
                return instance;
            }

            throw new RequestBodyNotValidException("Invalid Payload") { StatusCode = HttpStatusCode.BadRequest };
        }
    }

    /// <summary>
    /// this represents the validator entity for the SendMessages request body parameters.
    /// </summary>
    public class SendMessagesRequestBodyValidator : AbstractValidator<SendMessagesRequestBody>
    {
        private readonly IRegexDateTimeWrapper _regex;

        /// <summary>
        /// Initializes a new instance of the <see cref="SendMessagesRequestBodyValidator"/> class.
        /// </summary>
        ///
        public SendMessagesRequestBodyValidator(IRegexDateTimeWrapper regex)
        {
            this._regex = regex.ThrowIfNullOrDefault();

            this.RuleFor(p => p.TemplateId).MaximumLength(50).When(p => p.TemplateId.IsNullOrWhiteSpace() == false);
            this.RuleFor(p => p.Title).NotEmpty().MaximumLength(120);
            this.RuleFor(p => p.Body).NotEmpty().MaximumLength(4000);
            this.RuleFor(p => p.SenderNumber).NotEmpty().MaximumLength(13);
            this.RuleFor(p => p.RequestDate).Must(IsValidDateFormat).When(p => p.RequestDate.IsNullOrWhiteSpace() == false);
            this.RuleFor(p => p.SenderGroupingKey).MaximumLength(100).When(p => p.SenderGroupingKey.IsNullOrWhiteSpace() == false);
            this.RuleForEach(p => p.Recipients).SetValidator(new SendMessagesRequestRecipientValidator());
            this.RuleFor(p => p.UserId).MaximumLength(100).When(p => p.UserId.IsNullOrWhiteSpace() == false);
            this.RuleFor(p => p.StatsId).MaximumLength(100).When(p => p.StatsId.IsNullOrWhiteSpace() == false);
            this.RuleFor(p => p.OriginCode).MaximumLength(10).When(p => p.OriginCode.IsNullOrWhiteSpace() == false);
        }

        private bool IsValidDateFormat(string date)
        {
            return this._regex.IsMatch(date);
        }
    }

    /// <summary>
    /// this represents the validator entity for the SendMessages request recipient parameters.
    /// </summary>
    public class SendMessagesRequestRecipientValidator : AbstractValidator<SendMessagesRequestRecipient>
    {
        private readonly string _templateId;

        public SendMessagesRequestRecipientValidator(string templateId = null)
        {
            this._templateId = templateId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SendMessagesRequestRecipientValidator"/> class.
        /// </summary>
        public SendMessagesRequestRecipientValidator()
        {
            this.RuleFor(p => p.RecipientNumber).NotEmpty().MaximumLength(20);
            this.RuleFor(p => p.CountryCode).MaximumLength(8).When(p => p.CountryCode.IsNullOrWhiteSpace() == false);
            this.RuleFor(p => p.InternationalRecipientNumber).MaximumLength(20).When(p => p.InternationalRecipientNumber.IsNullOrWhiteSpace() == false);
            this.RuleFor(p => p.TemplateParameters).NotEmpty().When(_ => this._templateId.IsNullOrWhiteSpace() == false);
            this.RuleFor(p => p.RecipientGroupingKey).MaximumLength(1000).When(p => p.RecipientGroupingKey.IsNullOrWhiteSpace() == false);
        }
    }
}