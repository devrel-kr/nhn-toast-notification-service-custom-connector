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
        public void Given_RequestUrlBuilder_Type_Then_It_Should_Contain_Properties()
        {
            var fis = typeof(RequestUrlBuilder).GetProperties(BindingFlags.NonPublic|BindingFlags.Instance);

            fis.SingleOrDefault(p => p.Name == "_settings").Should().NotBeNull()
                .And.Subject.PropertyType.Should().Be(typeof(ToastSettings));

            fis.SingleOrDefault(p => p.Name == "_baseUrl").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(string));

            fis.SingleOrDefault(p => p.Name == "_version").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(string));

            fis.SingleOrDefault(p => p.Name == "_appKey").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(string));

            fis.SingleOrDefault(p => p.Name == "_endpoint").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(string));

            fis.SingleOrDefault(p => p.Name == "_queries").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(string));

            fis.SingleOrDefault(p => p.Name == "_paths").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(Dictionary<string, string>));
        }

        [DataTestMethod]
        [DataRow("baseUrl", "version", "endpoint")]
        [DataRow(null, null, null)]
        [DataRow(null, "version", "endpoint")]
        [DataRow("baseUrl", null, "endpoint")]
        [DataRow("baseUrl", "version", null)]
        public void Given_Parameters_When_WithSettins_Invoked_Then_It_Should_Return_Result(string baseUrl, string version, string endpoint)
        {
            var settings = new ToastSettings() { BaseUrl = baseUrl, Version = version };
            var result = new RequestUrlBuilder().WithSettings(settings, endpoint);

            var resultSetting = typeof(RequestUrlBuilder).GetProperty("_settings", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(result);
            var resultBaseUrl = typeof(RequestUrlBuilder).GetProperty("_baseUrl", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(result);
            var resultVersion = typeof(RequestUrlBuilder).GetProperty("_version", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(result);
            var resultEndpoint = typeof(RequestUrlBuilder).GetProperty("_endpoint", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(result);

            result.Should().BeOfType(typeof(RequestUrlBuilder));
            resultSetting.Should().Be(resultSetting);
            resultBaseUrl.Should().Be(baseUrl);
            resultVersion.Should().Be(version);
            resultEndpoint.Should().Be(endpoint);
        }

        [DataTestMethod]
        [DataRow("appKey")]
        [DataRow(null)]
        public void Given_Parameters_When_WithHeaders_Invoked_Then_It_Should_Return_Result(string appKey)
        {
            var headers = new RequestHeaderModel() { AppKey = appKey };
            var result = new RequestUrlBuilder().WithHeaders(headers);
            var resultAppKey = typeof(RequestUrlBuilder).GetProperty("_appKey", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(result);

            resultAppKey.Should().Be(appKey);
        }

        /*[TestMethod]
        public void Given_InvalidQueries_When_WithQueries_Invoked_Then_It_Should_Throw_Exception( )
        {
            var settings = new ToastSettings();
            var queries = new FakeRequestQuries2();
            var result = new RequestUrlBuilder().WithSettings(settings, null).WithQueries(queries);

            var resultQuery = typeof(RequestUrlBuilder).GetProperty("_queries", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(result);

            result.Should().BeOfType<RequestUrlBuilder>();
            resultQuery.Should().BeNull();
        }*/

        [TestMethod]
        public void Given_Default_Queries_Instance_When_WithQueries_Invoked_Then_It_Should_Return_Result()
        {
            var queries = new FakeRequestQuries();
            var result = new RequestUrlBuilder().WithQueries(queries);
            var resultQuery = typeof(RequestUrlBuilder).GetProperty("_queries", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(result);
            
            result.Should().BeOfType<RequestUrlBuilder>();
            resultQuery.Should().BeNull();
        }

        [TestMethod]
        public void Given_Default_Paths_Instance_When_WithPaths_Invoked_Then_It_Should_Return_Result()
        {
            var paths = new FakeRequestPaths();
            var result = new RequestUrlBuilder().WithPaths(paths);

            var resultQuery = typeof(RequestUrlBuilder).GetProperty("_paths", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(result);

            result.Should().BeOfType<RequestUrlBuilder>();
            resultQuery.Should().BeNull();
        }

        [TestMethod]
        public void Given_Default_RequestUrlBuilder_Instance_When_Build_Invoked_Then_It_Should_Return_Result()
        {
            var result = new RequestUrlBuilder().Build();

            result.Should().BeOfType(typeof(string));
        }
    }
}