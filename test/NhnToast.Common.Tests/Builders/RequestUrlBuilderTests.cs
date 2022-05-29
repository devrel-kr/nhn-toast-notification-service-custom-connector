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
            var settingsFi = typeof(RequestUrlBuilder).GetField("_settings", BindingFlags.NonPublic | BindingFlags.Instance);
            var endpointFi = typeof(RequestUrlBuilder).GetField("_endpoint", BindingFlags.NonPublic | BindingFlags.Instance);
            var queriesFi = typeof(RequestUrlBuilder).GetField("_queries", BindingFlags.NonPublic | BindingFlags.Instance);
            var pathsFi = typeof(RequestUrlBuilder).GetField("_paths", BindingFlags.NonPublic | BindingFlags.Instance);

            if (settingsFi != null)
            {
                settingsFi.FieldType.Should().Be(typeof(ToastSettings));
            }
            else 
            {
                throw new AssertFailedException("Can't find Field _settings in RequestUrlBuilder instance");
            }
            
            if (endpointFi != null)
            {
                endpointFi.FieldType.Should().Be(typeof(string));
            }
            else
            {
                throw new AssertFailedException("Can't find Field _endpoint in RequestUrlBuilder instance");
            }

            if (queriesFi != null)
            {
                queriesFi.FieldType.Should().Be(typeof(string));
            }
            else
            {
                throw new AssertFailedException("Can't find Field _queries in RequestUrlBuilder instance");
            }

            if (pathsFi != null) 
            {
                pathsFi.FieldType.Should().Be(typeof(Dictionary<string, string>));
            }
            else
            {
                throw new AssertFailedException("Can't find Field _paths in RequestUrlBuilder instance");
            }
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
            var settings = new ToastSettings() { };
            string endpoint = "";
            var queries = new FakeRequestQuries();
            var result = new RequestUrlBuilder().WithSettings(settings, endpoint).WithQueries(queries);
            var resultQueries = typeof(RequestUrlBuilder).GetField("_queries", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(result);
            
            result.Should().BeOfType<RequestUrlBuilder>();
            resultQueries.Should().Be("");
        }

        [TestMethod]
        public void Given_Default_Paths_Instance_When_WithPaths_Invoked_Then_It_Should_Return_Result()
        {
            var settings = new ToastSettings() { };
            string endpoint = "";
            var paths = new FakeRequestPaths();
            var result = new RequestUrlBuilder().WithSettings(settings,endpoint).WithPaths(paths);
            var resultPaths = typeof(RequestUrlBuilder).GetField("_paths", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(result);

            result.Should().BeOfType<RequestUrlBuilder>();
            resultPaths.Should().BeEquivalentTo(new Dictionary<string, string>());
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