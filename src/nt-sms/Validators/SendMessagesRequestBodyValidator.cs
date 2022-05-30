using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using FluentValidation;

using Toast.Common.Exceptions;
using Toast.Sms.Models;

namespace Toast.Sms.Validators
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
        /// <summary>
        /// Initializes a new instance of the <see cref="SendMessagesRequestBodyValidator"/> class.
        /// </summary>
        ///
        public SendMessagesRequestBodyValidator()
        {
            this.RuleFor(p => p.TemplateId).MaximumLength(50).When(p => p.TemplateId != null);
            this.RuleFor(p => p.Body).NotNull().MaximumLength(255);
            this.RuleFor(p => p.SenderNumber).NotEmpty().MaximumLength(13);
            this.RuleFor(p => p.RequestDate).Must(IsValidDateFormat).When(p => !string.IsNullOrWhiteSpace(p.RequestDate));
            this.RuleFor(p => p.SenderGroupingKey).MaximumLength(100);
            this.RuleForEach(p => p.Recipients).SetValidator(new SendMessagesRequestRecipientValidator());
            this.RuleFor(p => p.UserId).MaximumLength(100).When(p => p.UserId != null);
            this.RuleFor(p => p.StatsId).MaximumLength(100).When(p => p.StatsId != null);
        }

        private bool IsValidDateFormat(string date)
        {
            return Regex.IsMatch(date, @"^([0-9][0-9][0-9][0-9])-(0[1-9]|1[012])-(0[1-9]|[12][0-9]|3[0-1]) ([01][0-9]|2[0123]):([0-5][0-9]):([0-5][0-9])$");
        }
    }
    /// <summary>
    /// this represents the validator entity for the SendMessages request recipient parameters.
    /// </summary>
    public class SendMessagesRequestRecipientValidator : AbstractValidator<SendMessagesRequestRecipient>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SendMessagesRequestRecipientValidator"/> class.
        /// </summary>
        public SendMessagesRequestRecipientValidator()
        {
            this.RuleFor(p => p.RecipientNumber).NotEmpty().MaximumLength(20);
            this.RuleFor(p => p.CountryCode).MaximumLength(8).When(p => p.CountryCode != null);
            this.RuleFor(p => p.InternationalRecipientNumber).MaximumLength(20).When(p => p.InternationalRecipientNumber != null);
            this.RuleFor(p => p.RecipientGroupingKey).MaximumLength(100).When(p => p.RecipientGroupingKey != null);
        }
    }
}