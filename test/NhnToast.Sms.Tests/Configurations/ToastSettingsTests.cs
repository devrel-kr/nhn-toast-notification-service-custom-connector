using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Toast.Sms.Configurations;

namespace Toast.Sms.Tests.Configurations
{
    [TestClass]
    public class ToastSettingsTests
    {
        [DataTestMethod]
        [DataRow("1.0.0", "helloworld", "/api/{version}/apps/{appKey}", "/api/1.0.0/apps/helloworld")]
        [DataRow("1.0.0", "helloworld", "/api/{Version}/apps/{AppKey}", "/api/1.0.0/apps/helloworld")]
        public void Given_CarmelCaseObject_When_Formatted_Then_It_Should_Return_Result(string version, string appKey, string endpoint, string expected)
        {
            var options = new
            {
                version = version,
                appKey = appKey,
            };

            var settings = new ToastSettings();

            var result = settings.Formatter.Format(endpoint, options);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow("1.0.0", "helloworld", "/api/{version}/apps/{appKey}", "/api/1.0.0/apps/helloworld")]
        [DataRow("1.0.0", "helloworld", "/api/{Version}/apps/{AppKey}", "/api/1.0.0/apps/helloworld")]
        public void Given_CapitalCaseObject_When_Formatted_Then_It_Should_Return_Result(string version, string appKey, string endpoint, string expected)
        {
            var options = new
            {
                Version = version,
                AppKey = appKey,
            };

            var settings = new ToastSettings();

            var result = settings.Formatter.Format(endpoint, options);

            result.Should().Be(expected);
        }
    }
}