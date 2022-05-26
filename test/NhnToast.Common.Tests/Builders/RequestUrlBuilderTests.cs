using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Toast.Common.Builders;
using Toast.Common.Configurations;
using Toast.Common.Models;
using Toast.Common.Tests.Fakes;


namespace Toast.Common.Tests.Builders
{
    [TestClass]
    public class RequestBuilderValidatorTests
    {
        [TestMethod]
        public void Given_RequestUrlBuilder_Type_Then_It_Should_Contain_Fields()
        {
            var SettingsFi = typeof(RequestUrlBuilder).GetField("_settings", BindingFlags.NonPublic | BindingFlags.Instance);
            var EndpointFi = typeof(RequestUrlBuilder).GetField("_endpoint", BindingFlags.NonPublic | BindingFlags.Instance);
            var QueriesFi = typeof(RequestUrlBuilder).GetField("_queries", BindingFlags.NonPublic | BindingFlags.Instance);
            var PathsFi = typeof(RequestUrlBuilder).GetField("_paths", BindingFlags.NonPublic | BindingFlags.Instance);

            SettingsFi?.FieldType.Name.Should().Be("ToastSettings");
            EndpointFi?.FieldType.Name.Should().Be("String");
            QueriesFi?.FieldType.Name.Should().Be("String");
            PathsFi?.FieldType.Name.Should().Be("Dictionary`2");
        }

        [TestMethod]
        public void Given_Default_Settings_When_WithSettins_Invoked_Then_It_Should_Return_Result()
        {
            var settings = new ToastSettings() { };
            string endpoint = "";

            var result = new RequestUrlBuilder().WithSettings(settings, endpoint);
            var resultSetting = typeof(RequestUrlBuilder).GetField("_settings", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(result);
            var resultEndpoint = typeof(RequestUrlBuilder).GetField("_endpoint", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(result);

            result.Should().BeOfType(typeof(RequestUrlBuilder));
            resultSetting.Should().Be(settings);
            resultEndpoint.Should().Be(endpoint);
        }

        [TestMethod]
        public void Given_Default_Headers_When_WithHeaders_Invoked_Then_It_Should_Return_Result()
        {
            var headers = new RequestHeaderModel() {};
            var result = new RequestUrlBuilder().WithHeaders(headers);
            var resultHeader = typeof(RequestUrlBuilder).GetField("_headers", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(result);

            result.Should().BeOfType(typeof(RequestUrlBuilder));
            resultHeader.Should().Be(headers);
        }

        [TestMethod]
        public void Given_Default_Queries_Instance_When_WithQueries_Invoked_Then_It_Should_Return_Result()
        {
            var queries = new FakeRequestQuries();
            var result = new RequestUrlBuilder().WithQueries(queries);
            
            result.Should().BeOfType<RequestUrlBuilder>();
        }

        [TestMethod]
        public void Given_Default_Paths_Instance_When_WithPaths_Invoked_Then_It_Should_Return_Result()
        {
            var paths = new FakeRequestPaths();
            var result = new RequestUrlBuilder().WithPaths(paths);

            result.Should().BeOfType<RequestUrlBuilder>();
        }

        [DataTestMethod]
        [DataRow("https://api-sms.cloud.toast.com/", "/sms/{version}/appKeys/{appKey}/sender/sms", "v3.0", "appKey", "https://api-sms.cloud.toast.com/sms/v3.0/appKeys/appKey/sender/sms")]
        public void Given_Default_RequestUrlBuilder_Instance_When_Build_Invoked_Then_It_Should_Return_Result(string baseUrl, string endpoint, string version, string appKey, string expected)
        {
            var settings = new ToastSettings() { BaseUrl = baseUrl, Version = version };
            var headers = new RequestHeaderModel() { AppKey = appKey};
            var queries = new FakeRequestQuries();
            var paths = new FakeRequestPaths();

            var result = new RequestUrlBuilder()
                .WithSettings(settings, endpoint)
                .WithHeaders(headers)
                .WithQueries(queries)
                .WithPaths(paths)
                .Build();

            result.Should().Be(expected);
        }
    }
}