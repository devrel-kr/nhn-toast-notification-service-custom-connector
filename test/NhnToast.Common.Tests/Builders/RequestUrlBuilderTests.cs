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
        [DataRow("BaseUrl", "Version", "Endpoint")]
        [DataRow(null, null, null)]
        [DataRow(null, "Version", "Endpoint")]
        [DataRow("BaseUrl", null, "Endpoint")]
        [DataRow("BaseUrl", "Version", null)]
        public void Given_Parameters_When_WithSettins_Invoked_Then_It_Should_Return_Result(string baseUrl, string version, string endpoint)
        {
            var settings = new ToastSettings() { BaseUrl = baseUrl, Version = version };
            var result = new RequestUrlBuilder().WithSettings(settings, endpoint);

            var builder = typeof(RequestUrlBuilder);
            
            var resultBaseUrl = typeof(RequestUrlBuilder).GetField("BaseUrl", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(result);
            var resultVersion = typeof(RequestUrlBuilder).GetField("Version", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(result);
            
            resultBaseUrl.Should().Be(baseUrl);
            resultVersion.Should().Be(version);
        }

        [DataTestMethod]
        [DataRow("AppKey")]
        [DataRow(null)]
        public void Given_Parameters_When_WithHeaders_Invoked_Then_It_Should_Return_Result(string appKey)
        {
            var headers = new RequestHeaderModel() { AppKey = appKey };
            var result = new RequestUrlBuilder().WithHeaders(headers);

            var resultAppKey = typeof(RequestUrlBuilder).GetField("AppKey", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(result);

            resultAppKey.Should().Be(appKey);
        }

        [DataTestMethod]
        [DataRow("This is Query1", "This is Query2", "query1=This is Query1&query2=This is Query2")]
        [DataRow(null, null, "")]
        [DataRow(null, "This is Query2", "query2=This is Query2")]
        public void Given_Parameters_When_WithQueries_Invoked_Then_It_Should_Return_Result(string value1, string value2, string expected)
        {
            var settings = new ToastSettings();
            var queries = new BaseRequestQueries() { Query1 = value1, Query2 = value2 };
            var result = new RequestUrlBuilder().WithSettings(settings, "").WithQueries(queries);

            var resultQuery = typeof(RequestUrlBuilder).GetField("Queries", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(result);
            
            resultQuery.Should().Be(expected);
            //var query = typeof(RequestUrlBuilder).GetField("Queries", BindingFlags.Public);
            //var resultQuery = query?.GetValue(result) as Dictionary<string, string>;
        }

        [DataTestMethod]
        [DataRow("This is Path1", "This is Path2", "This is Path1/This is Path2")]
        [DataRow(null, null, "")]
        [DataRow(null, "This is Path2", "This is Path2")]
        public void Given_Parameters_When_WithPaths_Invoked_Then_It_Should_Return_Result(string value1, string value2, string expected)
        {
            var settings = new ToastSettings();
            var paths = new BaseRequestPaths() { Path1 = value1, Path2 = value2 };
            var result = new RequestUrlBuilder().WithSettings(settings, "").WithPaths(paths);

            var resultPath = typeof(RequestUrlBuilder).GetField("Paths", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(result);

            resultPath.Should().Be(expected);
            //var path = typeof(RequestUrlBuilder).GetField("Queries", BindingFlags.Public);
            //var resultPath = path?.GetValue(result) as Dictionary<string, string>;
        }

        [DataTestMethod]
        [DataRow("https://api-sms.cloud.toast.com/", "/sms/{version}/appKeys/{appKey}/sender/sms", "v3.0", "appKey", "path1", "path2", "query1", "query2", "https://api-sms.cloud.toast.com/sms/v3.0/appKeys/appKey/sender/sms/path1/path2?query1=query1&query2=query2")]
        [DataRow(null, "/sms/{version}/appKeys/{appKey}/sender/sms", "v3.0", "appKey", "path1", "path2", "query1", "query2", "/sms/{version}/appKeys/{appKey}/sender/sms")]
        [DataRow("https://api-sms.cloud.toast.com/", null, "v3.0", "appKey", "path1", "path2", "query1", "query2", "https://api-sms.cloud.toast.com/")]
        [DataRow("https://api-sms.cloud.toast.com/", "/sms/{version}/appKeys/{appKey}/sender/sms", null, "appKey", "path1", "path2", "query1", "query2", "https://api-sms.cloud.toast.com/sms//appKeys/appKey/sender/sms/path1/path2?query1=query1&query2=query2")]
        [DataRow("https://api-sms.cloud.toast.com/", "/sms/{version}/appKeys/{appKey}/sender/sms", "v3.0", null, "path1", "path2", "query1", "query2", "https://api-sms.cloud.toast.com/sms/v3.0/appKeys//sender/sms/path1/path2?query1=query1&query2=query2")]
        [DataRow("https://api-sms.cloud.toast.com/", "/sms/{version}/appKeys/{appKey}/sender/sms", "v3.0", "appKey", null, "path2", "query1", "query2", "https://api-sms.cloud.toast.com/sms/v3.0/appKeys/appKey/sender/sms/path2?query1=query1&query2=query2")]
        [DataRow("https://api-sms.cloud.toast.com/", "/sms/{version}/appKeys/{appKey}/sender/sms", "v3.0", "appKey", "path1", null, "query1", "query2", "https://api-sms.cloud.toast.com/sms/v3.0/appKeys/appKey/sender/sms/path1?query1=query1&query2=query2")]
        [DataRow("https://api-sms.cloud.toast.com/", "/sms/{version}/appKeys/{appKey}/sender/sms", "v3.0", "appKey", "path1", "path2", null, "query2", "https://api-sms.cloud.toast.com/sms/v3.0/appKeys/appKey/sender/sms/path1/path2?query2=query2")]
        [DataRow("https://api-sms.cloud.toast.com/", "/sms/{version}/appKeys/{appKey}/sender/sms", "v3.0", "appKey", "path1", "path2", "query1", null, "https://api-sms.cloud.toast.com/sms/v3.0/appKeys/appKey/sender/sms/path1/path2?query1=query1")]
        [DataRow("https://api-sms.cloud.toast.com/", "/sms/{version}/appKeys/{appKey}/sender/sms", "v3.0", "appKey", null, null, null, null, "https://api-sms.cloud.toast.com/sms/v3.0/appKeys/appKey/sender/sms")]
        public void Given_Parameters_When_Builder_Invoked_Then_It_Should_Return_Result(string baseUrl, string endpoint, string version, string appKey, string pathValue1, string pathValue2, string queryValue1, string queryValue2, string expected)
        {
            var settings = new ToastSettings() { BaseUrl = baseUrl, Version = version };
            var headers = new RequestHeaderModel() { AppKey = appKey };
            var queries = new BaseRequestQueries() { Query1 = queryValue1, Query2 = queryValue2 };
            var paths = new BaseRequestPaths() { Path1 = pathValue1, Path2 = pathValue2 };

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