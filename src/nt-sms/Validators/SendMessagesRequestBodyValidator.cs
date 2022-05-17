using System.Linq;
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

            throw new RequestBodyNotValidException("Not Found") { StatusCode = System.Net.HttpStatusCode.BadRequest };
        }
    }

    /// <summary>
    /// this represents the validator entity for the GetMessage request query parameters.
    /// </summary>
    public class SendMessagesRequestBodyValidator : AbstractValidator<SendMessagesRequestBody>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SendMessagesRequestBodyValidator"/> class.
        /// </summary>
        public SendMessagesRequestBodyValidator()
        {
            this.RuleFor(p => p.Body).NotNull();
            this.RuleFor(p => p.SenderNumber).NotEmpty();
            this.RuleFor(p => p.Recipients.Count(q => !string.IsNullOrWhiteSpace(q.RecipientNumber))).Equals(0);
        }
    }
}