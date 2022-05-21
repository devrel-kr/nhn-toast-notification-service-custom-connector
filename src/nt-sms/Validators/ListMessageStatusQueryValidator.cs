using System.Net;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

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

            throw new RequestQueryNotValidException("Not Found") { StatusCode = HttpStatusCode.BadRequest };
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
            this.RuleFor(p => p.StartUpdateDate).Must(IsValidDateFormat).NotEmpty();
            this.RuleFor(p => p.EndUpdateDate).Must(IsValidDateFormat).NotEmpty().GreaterThan(q => q.StartUpdateDate);
            this.RuleFor(p => p.PageNumber).GreaterThan(0);
            this.RuleFor(p => p.PageSize).GreaterThan(0);
            this.RuleFor(p => p.MessageType);

        }
        private bool IsValidDateFormat(string date)
        {
            if (string.IsNullOrWhiteSpace(date))
            {
                return false;
            }
            else
                return Regex.IsMatch(date, @"^([0-9]{4})-(0[1-9]|1[012])-(0[1-9]|[12][0-9]|3[0-1]) ([01][0-9]|2[0123]):([0-5][0-9]):([0-5][0-9])$");
        }
        private bool IsValidMessageType(string messageType)
        {
            if (string.IsNullOrWhiteSpace(messageType)) return false;
            else if (messageType.Equals("SMS")) return true;
            else if (messageType.Equals("LMS")) return true;
            else if (messageType.Equals("MMS")) return true;
            else if (messageType.Equals("AUTH")) return true;
            else return false;
        }
    }
}