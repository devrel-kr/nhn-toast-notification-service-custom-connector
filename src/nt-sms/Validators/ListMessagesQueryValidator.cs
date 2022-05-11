using System.Threading.Tasks;

using FluentValidation;

using Toast.Common.Exceptions;
using Toast.Sms.Models;

namespace Toast.Sms.Validators
{
    /// <summary>
    /// This represents the extension entity for the request header.
    /// </summary>
    public static class ListMessagesQueryValidatorExtension
    {
        /// <summary>
        /// Validates the request query parameters against ListMessages.
        /// </summary>
        /// <param name="headers"><see cref="ListMessagesRequestQueries"/> instance.</param>
        /// <returns>Returns the <see cref="ListMessagesRequestQueries"/> instance.</returns>
        public static async Task<ListMessagesRequestQueries> Validate(this Task<ListMessagesRequestQueries> queries, IValidator<ListMessagesRequestQueries> validator)
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
    public class ListMessagesRequestQueryValidator : AbstractValidator<ListMessagesRequestQueries>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListMessagesRequestQueryValidator"/> class.
        /// </summary>
        public ListMessagesRequestQueryValidator()
        {
            
            this.RuleFor(p => p.RequestId)
                .NotEmpty().When(p=>p.StartRequestDate == null, ApplyConditionTo.CurrentValidator).When(p=>p.EndRequestDate == null, ApplyConditionTo.CurrentValidator).When(p => p.StartCreateDate == null).When(p => p.EndCreateDate == null, ApplyConditionTo.CurrentValidator);

            this.RuleFor(p => p.StartRequestDate)
                .NotEmpty().When(p => p.RequestId == null, ApplyConditionTo.CurrentValidator).When(p => p.StartCreateDate == null, ApplyConditionTo.CurrentValidator).When(p => p.EndCreateDate == null, ApplyConditionTo.CurrentValidator);

            this.RuleFor(p => p.EndRequestDate)
                .NotEmpty().When(p => p.RequestId == null, ApplyConditionTo.CurrentValidator).When(p => p.StartCreateDate == null, ApplyConditionTo.CurrentValidator).When(p => p.EndCreateDate == null, ApplyConditionTo.CurrentValidator);

            this.RuleFor(p => p.StartCreateDate)
                .NotEmpty().When(p => p.RequestId == null, ApplyConditionTo.CurrentValidator).When(p => p.StartRequestDate == null, ApplyConditionTo.CurrentValidator).When(p => p.EndRequestDate == null, ApplyConditionTo.CurrentValidator);

            this.RuleFor(p => p.EndCreateDate)
                .NotEmpty().When(p => p.RequestId == null, ApplyConditionTo.CurrentValidator).When(p => p.StartRequestDate == null, ApplyConditionTo.CurrentValidator).When(p => p.EndRequestDate == null, ApplyConditionTo.CurrentValidator);
            
            this.RuleFor(p => p.PageNumber).GreaterThanOrEqualTo(1);
            this.RuleFor(p => p.PageSize).GreaterThanOrEqualTo(1);
        }
    }
}