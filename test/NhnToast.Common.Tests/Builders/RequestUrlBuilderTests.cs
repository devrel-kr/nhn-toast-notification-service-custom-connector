using System;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Toast.Common.Builders;
using Toast.Common.Configurations;
using Toast.Common.Exceptions;
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

            result.BaseUrl.Should().Be(baseUrl);
            result.Version.Should().Be(version);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("AppKey")]
        public void Given_Parameters_When_WithHeaders_Invoked_Then_It_Should_Return_Result(string appKey)
        {
            var headers = new RequestHeaderModel() { AppKey = appKey };

            var result = new RequestUrlBuilder().WithHeaders(headers);

            result.AppKey.Should().Be(appKey);
        }

        [DataTestMethod]
        [DataRow(null, null, "Value cannot be null. (Parameter 'name')")]
        [DataRow("Key1", "Value1", null)]
        public void Given_Parameters_When_WithQueries_Invoked_Then_It_Should_Return_Result(string key, string value, string expected)
        {
            string? exceptionMessage = null;

            try
            {
                var queries = new JObject();
                queries.Add(key, value);

                var result = new RequestUrlBuilder().WithQueries(queries);

                result.Queries.Keys.Should().Contain(key);
                result.Queries.Values.Should().Contain(value);
            }
            catch (Exception ex)
            {
                exceptionMessage = ex.Message;
            }

            exceptionMessage.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(null, null, "Object reference not set to an instance of an object.")]
        [DataRow("Key", "Value", null)]
        public void Given_Parameters_When_WithPaths_Invoked_Then_It_Should_Return_Result(string key, string value, string expected)
        {
            string? exceptionMessage = null;

            try
            {
                var paths = new string[] { key, value };

                var result = new RequestUrlBuilder().WithPaths(paths);

                for (int i = 0; i < paths.Length; i += 2)
                {
                    result.Paths.Keys.Should().Contain(paths[i]);
                    result.Paths.Values.Should().Contain(paths[i + 1]);
                }
            }
            catch (Exception ex)
            {
                exceptionMessage = ex.Message;
            }

            exceptionMessage?.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow("This is BaseUrl", "This is Version", "This is AppKey", "{version}/appKey/{appKey}/path/{path}/query/{query}", "query", "This is Query", "path", "This is Path", "This is BaseUrl/This is Version/appKey/This is AppKey/path/This is Path/query/This is Query")]
        [DataRow(null, "This is Version", "This is AppKey", "{version}/appKey/{appKey}/path/{path}/query/{query}", "query", "This is Query", "path", "This is Path", "/{version}/appKey/{appKey}/path/{path}/query/{query}")]
        [DataRow("This is BaseUrl", null, "This is AppKey", "{version}/appKey/{appKey}/path/{path}/query/{query}", "query", "This is Query", "path", "This is Path", "This is BaseUrl/appKey/This is AppKey/path/This is Path/query/This is Query")]
        [DataRow("This is BaseUrl", "This is Version", null, "{version}/appKey/{appKey}/path/{path}/query/{query}", "query", "This is Query", "path", "This is Path", "This is BaseUrl/This is Version/appKey//path/This is Path/query/This is Query")]
        [DataRow("This is BaseUrl", "This is Version", "This is AppKey", null, "query", "This is Query", "path", "This is Path", "This is BaseUrl/")]
        [DataRow("This is BaseUrl", "This is Version", "This is AppKey", "{version}/appKey/{appKey}/path/{path}/query/{query}", null, "This is Query", "path", "This is Path", "This is BaseUrl/This is Version/appKey/This is AppKey/path/This is Path/query/{query}")]
        [DataRow("This is BaseUrl", "This is Version", "This is AppKey", "{version}/appKey/{appKey}/path/{path}/query/{query}", "query", null, "path", "This is Path", "This is BaseUrl/This is Version/appKey/This is AppKey/path/This is Path/query/")]
        [DataRow("This is BaseUrl", "This is Version", "This is AppKey", "{version}/appKey/{appKey}/path/{path}/query/{query}", "query", "This is Query", null, "This is Path", "This is BaseUrl/This is Version/appKey/This is AppKey/path/{path}/query/This is Query")]
        [DataRow("This is BaseUrl", "This is Version", "This is AppKey", "{version}/appKey/{appKey}/path/{path}/query/{query}", "query", "This is Query", "path", null, "This is BaseUrl/This is Version/appKey/This is AppKey/path//query/This is Query")]
        public void Given_Parameters_When_Builder_Invoked_Then_It_Should_Return_Result(string baseUrl, string version, string appKey, string endpoint, string queryKey, string queryValue, string pahtKey, string pathValue, string expected)
        {
            var settings = new ToastSettings() { BaseUrl = baseUrl, Version = version };
            var headers = new RequestHeaderModel() { AppKey = appKey };
            var queries = new JObject();
            var paths = new string[] { pahtKey, pathValue };
            try
            {
                queries.Add(queryKey, queryValue);
            }
            catch (Exception ex)
            {
                queries = null;
            }
            
            

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