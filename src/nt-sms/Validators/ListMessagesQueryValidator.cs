using System.Collections.Generic;
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

            throw new RequestQueryNotValidException("Invalid Query Parameter") { StatusCode = HttpStatusCode.BadRequest };
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
                .NotEmpty()
                .MaximumLength(25)
                .When(p => p.StartRequestDate == null)
                .When(p => p.EndRequestDate == null)
                .When(p => p.StartCreateDate == null)
                .When(p => p.EndCreateDate == null);

            this.RuleFor(p => p.StartRequestDate)
                .NotEmpty()
                .Must(IsValidDateFormat)
                .When(p => p.RequestId == null)
                .When(p => p.StartCreateDate == null)
                .When(p => p.EndCreateDate == null);

            this.RuleFor(p => p.EndRequestDate)
                .NotEmpty()
                .Must(IsValidDateFormat)
                .GreaterThan(p => p.StartRequestDate)
                .When(p => p.RequestId == null)
                .When(p => p.StartCreateDate == null)
                .When(p => p.EndCreateDate == null);

            this.RuleFor(p => p.StartCreateDate)
                .NotEmpty()
                .Must(IsValidDateFormat)
                .When(p => p.RequestId == null)
                .When(p => p.StartRequestDate == null)
                .When(p => p.EndRequestDate == null);

            this.RuleFor(p => p.EndCreateDate)
                .NotEmpty()
                .Must(IsValidDateFormat)
                .GreaterThan(p => p.StartCreateDate)
                .When(p => p.RequestId == null)
                .When(p => p.StartRequestDate == null)
                .When(p => p.EndRequestDate == null);

            this.RuleFor(p => p.StartResultDate).Must(IsValidDateFormat).When(p => p.StartResultDate != null);

            this.RuleFor(p => p.EndResultDate).Must(IsValidDateFormat).GreaterThan(p => p.StartResultDate).When(p => p.EndResultDate != null);

            this.RuleFor(p => p.SendNumber).MaximumLength(13);

            this.RuleFor(p => p.RecipientNumber).MaximumLength(20);

            this.RuleFor(p => p.TemplateId).MaximumLength(50);

            this.RuleFor(p => p.MessageStatus).MaximumLength(1).Must(p => MsgStatusType.Contains(p)).When(p => p.MessageStatus != null);

            this.RuleFor(p => p.ResultCode).MaximumLength(10).Must(p => resultCodeType.Contains(p)).When(p => p.ResultCode != null);

            this.RuleFor(p => p.SubResultCode).MaximumLength(10).Must(p => subResultCodeType.Contains(p)).When(p => p.SubResultCode != null);
            
            this.RuleFor(p => p.SenderGroupingKey).MaximumLength(100);

            this.RuleFor(p => p.RecipientGroupingKey).MaximumLength(100);

            this.RuleFor(p => p.PageNumber).GreaterThan(0);

            this.RuleFor(p => p.PageSize).GreaterThan(0);
        }

        List<string> MsgStatusType = new List<string>() { "0", "1", "2", "3", "4", "5" };
        List<string> resultCodeType = new List<string>() { "MTR1", "MTR2" };
        List<string> subResultCodeType = new List<string>() { "MTR2_1", "MTR2_2", "MTR2_3" };
        private bool IsValidDateFormat(string date)
        {
            if (date == null)
            {
                return false;
            }
            else
            {
                return Regex.IsMatch(date, @"^([0-9][0-9][0-9][0-9])-(0[1-9]|1[012])-(0[1-9]|[12][0-9]|3[0-1]) ([01][0-9]|2[0123]):([0-5][0-9]):([0-5][0-9])$");
            }
        }
    }
}