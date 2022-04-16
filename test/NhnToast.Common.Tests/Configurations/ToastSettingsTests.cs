using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Toast.Common.Configurations;
using Toast.Common.Tests.Fakes;

namespace Toast.Common.Tests.Configurations
{
    [TestClass]
    public class ToastSettingsTests
    {
        [DataTestMethod]
        [DataRow("1.0.0", "helloworld", "/api/{version}/apps/{appKey}", "/api/1.0.0/apps/helloworld")]
        [DataRow("1.0.0", "helloworld", "/api/{Version}/apps/{AppKey}", "/api/1.0.0/apps/helloworld")]
        public void Given_CarmelCaseProperties_When_Formatted_Then_It_Should_Return_Result(string version, string appKey, string endpoint, string expected)
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
        public void Given_CapitalCaseProperties_When_Formatted_Then_It_Should_Return_Result(string version, string appKey, string endpoint, string expected)
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

        [DataTestMethod]
        [DataRow("1.0.0", "helloworld", "/api/{version}/apps/{appKey}", "/api/1.0.0/apps/helloworld")]
        [DataRow("1.0.0", "helloworld", "/api/{Version}/apps/{AppKey}", "/api/1.0.0/apps/helloworld")]
        public void Given_CarmelCasePlaceholders_When_Formatted_Then_It_Should_Return_Result(string version, string appKey, string endpoint, string expected)
        {
            var options = new FakeRequestUrlOptions()
            {
                Version = version,
                AppKey = appKey,
            };

            var settings = new ToastSettings<FakeEndpointSettings>();

            var result = settings.Formatter.Format(endpoint, options);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow("1.0.0", "helloworld", "/api/{version}/apps/{appKey}", "/api/1.0.0/apps/helloworld")]
        [DataRow("1.0.0", "helloworld", "/api/{Version}/apps/{AppKey}", "/api/1.0.0/apps/helloworld")]
        public void Given_CapitalCasePlaceholders_When_Formatted_Then_It_Should_Return_Result(string version, string appKey, string endpoint, string expected)
        {
            var options = new FakeRequestUrlOptions()
            {
                Version = version,
                AppKey = appKey,
            };

            var settings = new ToastSettings<FakeEndpointSettings>();

            var result = settings.Formatter.Format(endpoint, options);

            result.Should().Be(expected);
        }
    }
}