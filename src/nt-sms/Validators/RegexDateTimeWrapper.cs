using System.Text.RegularExpressions;

namespace Toast.Sms.Validators
{
    /// <summary>
    /// This describes the method that checks validation of format in its contract.
    /// </summary>
    public interface IRegexDateTimeWrapper
    {
        /// <summary>
        /// Gets whether the date format is valid or not.
        /// </summary>
        bool IsMatch(string date);
    }

    /// <summary>
    /// This represents the singleton entity of regex for the validators.
    /// </summary>
    public class RegexDateTimeWrapper : IRegexDateTimeWrapper
    {
        private static Regex regex = new Regex(@"^([0-9][0-9][0-9][0-9])-(0[1-9]|1[012])-(0[1-9]|[12][0-9]|3[0-1]) ([01][0-9]|2[0123]):([0-5][0-9]):([0-5][0-9])$");

        /// <inheritdoc />
        public bool IsMatch(string date)
        {
            if (string.IsNullOrWhiteSpace(date))
            {
                return false;
            }
            
            return regex.IsMatch(date);
        }
    }
}