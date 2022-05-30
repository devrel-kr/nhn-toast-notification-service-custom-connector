using System.Collections.Generic;
using System.Reflection;

using FluentAssertions;

using Newtonsoft.Json;
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

            settingsFi.Should().NotBeNull();
            settingsFi.FieldType.Should().Be(typeof(ToastSettings));

            endpointFi.Should().NotBeNull();
            endpointFi.FieldType.Should().Be(typeof(string));

            queriesFi.Should().NotBeNull();
            queriesFi.FieldType.Should().Be(typeof(string));

            pathsFi.Should().NotBeNull();
            pathsFi.FieldType.Should().Be(typeof(Dictionary<string, string>));
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
            var headers = new RequestHeaderModel() { };
            var result = new RequestUrlBuilder().WithHeaders(headers);
            var resultHeader = typeof(RequestUrlBuilder).GetField("_headers", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(result);

            result.Should().BeOfType(typeof(RequestUrlBuilder));
            resultHeader.Should().Be(headers);
        }

        [DataTestMethod]
        [DataRow("FakeQuery1", "FakeQuery2", "fakeQuery1=FakeQuery1&fakeQuery2=FakeQuery2")]
        [DataRow("FakeQuery1", null, "fakeQuery1=FakeQuery1")]
        [DataRow("FakeQuery1", "", "fakeQuery1=FakeQuery1&fakeQuery2=")]
        public void Given_Fake_Queries_Instance_When_WithQueries_Invoked_Then_It_Should_Return_Result(string query1, string query2, string expected)
        {
            var settings = new ToastSettings() { };
            string endpoint = "";
            var queries = new FakeRequestQuries() {FakeQuery1 = query1, FakeQuery2 = query2};
            var result = new RequestUrlBuilder().WithSettings(settings, endpoint).WithQueries(queries);
            var resultQueries = typeof(RequestUrlBuilder).GetField("_queries", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(result);
            
            result.Should().BeOfType(typeof(RequestUrlBuilder));
            resultQueries.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow("FakePath", 1)]
        [DataRow(null, 0)]
        [DataRow("", 1)]
        public void Given_Fake_Paths_Instance_When_WithPaths_Invoked_Then_It_Should_Return_Result(string path, int count)
        {
            var settings = new ToastSettings() { };
            string endpoint = "";
            var paths = new FakeRequestPaths() {FakePaths=path};
            var result = new RequestUrlBuilder().WithSettings(settings,endpoint).WithPaths(paths);
            var resultPaths = typeof(RequestUrlBuilder).GetField("_paths", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(result) as Dictionary<string, string>;
            
            result.Should().BeOfType(typeof(RequestUrlBuilder));
            resultPaths.Count.Should().Be(count);

            resultPaths.Should().BeOfType(typeof(Dictionary<string, string>));
        }

        [DataTestMethod]
        [DataRow("https://api-sms.cloud.toast.com/", "/sms/{version}/appKeys/{appKey}/sender/sms/{fakePaths}", "v3.0", "appKey", "FakePaths","FakeQuery" ,"https://api-sms.cloud.toast.com/sms/v3.0/appKeys/appKey/sender/sms/FakePaths?fakeQuery1=FakeQuery")]
        public void Given_Default_RequestUrlBuilder_Instance_When_Build_Invoked_Then_It_Should_Return_Result(string baseUrl, string endpoint, string version, string appKey, string path,string query,string expected)
        {
            var settings = new ToastSettings() { BaseUrl = baseUrl, Version = version };
            var headers = new RequestHeaderModel() { AppKey = appKey};
            var queries = new FakeRequestQuries() { FakeQuery1 = query};
            var paths = new FakeRequestPaths() { FakePaths = path};

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