using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Toast.Common.Builders;
using Toast.Common.Configurations;
using Toast.Common.Models;
using Toast.Common.Validators;

namespace Toast.Common.Tests.Builders
{
    [TestClass]
    public class RequestBuilderValidatorTests
    {
        [DataTestMethod]
        [DataRow(null, null, null)]
        [DataRow(null, "Version", "Endpoint")]
        [DataRow("BaseUrl", null, "Endpoint")]
        [DataRow("BaseUrl", "Version", null)]
        [DataRow("BaseUrl", "Version", "Endpoint")]
        public void Given_Parameters_When_WithSettins_Invoked_Then_It_Should_Return_Result(string baseUrl, string version, string endpoint)
        {
            var settings = new ToastSettings() { BaseUrl = baseUrl, Version = version };
            var result = new RequestUrlBuilder().WithSettings(settings, endpoint);

            var builder = typeof(RequestUrlBuilder);
            var resultBaseUrl = builder.GetField("BaseUrl", BindingFlags.Public)?.GetValue(result);
            var resultVersion = builder.GetField("Version", BindingFlags.Public)?.GetValue(result);

            resultBaseUrl.Should().Be(baseUrl);
            resultVersion.Should().Be(version);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("AppKey")]
        public void Given_Parameters_When_WithHeaders_Invoked_Then_It_Should_Return_Result(string appKey)
        {
            var headers = new RequestHeaderModel() { AppKey = appKey };
            var result = new RequestUrlBuilder().WithHeaders(headers);
            var resultAppKey = typeof(RequestUrlBuilder).GetField("AppKey", BindingFlags.Public)?.GetValue(result);

            resultAppKey.Should().Be(appKey);
        }

        [DataTestMethod]
        [DataRow(null, null)]
        [DataRow("Key1", "Value1")]
        public void Given_Parameters_When_WithQueries_Invoked_Then_It_Should_Return_Result(string key, string value)
        {
            var queries = new BaseRequestQueries() { Name = key, Value = value };
            var result = new RequestUrlBuilder().WithQueries(queries);
            
            var query = typeof(RequestUrlBuilder).GetField("Queries", BindingFlags.Public);
            var resultQuery = query?.GetValue(result) as Dictionary<string, string>;

            resultQuery?.Keys.Should().Contain(key);
            resultQuery?.Values.Should().Contain(value);
        }

        [DataTestMethod]
        [DataRow(null, null)]
        [DataRow("Key1", "Value1")]
        public void Given_Parameters_When_WithPaths_Invoked_Then_It_Should_Return_Result(string key, string value)
        {
            var paths = new BaseRequestPaths() { Name = key, Value = value };
            var result = new RequestUrlBuilder().WithPaths(paths);

            var path = typeof(RequestUrlBuilder).GetField("Queries", BindingFlags.Public);
            var resultPath = path?.GetValue(result) as Dictionary<string, string>;

            resultPath?.Keys.Should().Contain(key);
            resultPath?.Values.Should().Contain(value);
        }

        [DataTestMethod]
        [DataRow("https://api-sms.cloud.toast.com/", "v3.0", "AppKey", "/sms/{version}/appKeys/{appKey}/sender/sms", "query1", "Query1", "query2", "Query2", "path", "Path", "https://api-sms.cloud.toast.com/sms/v3.0/appKeys/AppKey/sender/sms/Path?query1=Query1&query2=Query2")]
        [DataRow(null, "v3.0", "AppKey", "/sms/{version}/appKeys/{appKey}/sender/sms", "query", "Query", "path", "Path", "/sms/{version}/appKeys/{appKey}/sender/sms/Path?query1=Query1&query2=Query2")]
        [DataRow("https://api-sms.cloud.toast.com/", null, "AppKey", "/sms/{version}/appKeys/{appKey}/sender/sms", "query1", "Query1", "query2", "Query2", "path", "Path", "https://api-sms.cloud.toast.com/sms/appKeys/AppKey/sender/sms/Path??query1=Query1&query2=Query2")]
        [DataRow("https://api-sms.cloud.toast.com/", "v3.0", null, "/sms/{version}/appKeys/{appKey}/sender/sms", "query1", "Query1", "query2", "Query2", "path", "Path", "https://api-sms.cloud.toast.com/sms/v3.0/appKeys//sender/sms/Path?query1=Query1&query2=Query2")]
        [DataRow("https://api-sms.cloud.toast.com/", "v3.0", "AppKey", null, "query1", "Query1", "query2", "Query2", "path", "Path", "https://api-sms.cloud.toast.com/")]
        [DataRow("https://api-sms.cloud.toast.com/", "v3.0", "AppKey", "/sms/{version}/appKeys/{appKey}/sender/sms", "query1", null, "query2", "Query2", "path", "Path", "https://api-sms.cloud.toast.com/sms/v3.0/appKeys/AppKey/sender/sms/Path?query2=Query2")]
        [DataRow("https://api-sms.cloud.toast.com/", "v3.0", "AppKey", "/sms/{version}/appKeys/{appKey}/sender/sms", "query1", "Query1", "query2", null, "path", "Path", "https://api-sms.cloud.toast.com/sms/v3.0/appKeys/AppKey/sender/sms/Path?query1=Query1")]
        [DataRow("https://api-sms.cloud.toast.com/", "v3.0", "AppKey", "/sms/{version}/appKeys/{appKey}/sender/sms", "query1", "Query1", "query2", "Query2", "path", null, "https://api-sms.cloud.toast.com/sms/v3.0/appKeys/AppKey/sender/sms?query1=Query1&query2=Query2")]
        [DataRow("https://api-sms.cloud.toast.com/", "v3.0", "AppKey", "/sms/{version}/appKeys/{appKey}/sender/sms", "query1", null, null, null, null, null, "https://api-sms.cloud.toast.com/sms/v3.0/appKeys/AppKey/sender/sms")]

        public void Given_Parameters_When_Builder_Invoked_Then_It_Should_Return_Result(string baseUrl, string version, string appKey, string endpoint, string queryKey1, string queryValue1, string queryKey2, string queryValue2, string pahtKey, string pathValue, string expected)
        {
            var settings = new ToastSettings() { BaseUrl = baseUrl, Version = version };
            var headers = new RequestHeaderModel() { AppKey = appKey };
            var queries = new BaseRequestQueries() { Name1 = queryKey1, Name2 = queryKey2, Value1 = queryValue1, Value2 = queryValue2 };
            var paths = new BaseRequestPaths() { Name = pahtKey, Value = pathValue };

            var requestUrl = new RequestUrlBuilder()
             .WithSettings(settings, endpoint)
             .WithHeaders(headers)
             .WithQueries(queries)
             .WithPaths(paths)
             .Build();

            requestUrl.Should().Be(expected);
        }
    }
}