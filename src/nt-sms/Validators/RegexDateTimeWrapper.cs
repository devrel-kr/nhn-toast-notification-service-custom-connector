using System.Text.RegularExpressions;

namespace Toast.Sms.Validators
{
    /// <summary>
    /// This represents the singleton entity of regex for the validators.
    /// </summary>
    public interface IRegexDateTimeWrapper
    {
        bool IsMatch(string date);
    }

    public class RegexDateTimeWrapper : IRegexDateTimeWrapper
    {
        private static Regex _regex = new Regex(@"^([0-9][0-9][0-9][0-9])-(0[1-9]|1[012])-(0[1-9]|[12][0-9]|3[0-1]) ([01][0-9]|2[0123]):([0-5][0-9]):([0-5][0-9])$");

        public bool IsMatch(string date)
        {
            if (date == null)
            {
                return false;
            }
            else
            {
                return _regex.IsMatch(date);
            }
        }
    }
}