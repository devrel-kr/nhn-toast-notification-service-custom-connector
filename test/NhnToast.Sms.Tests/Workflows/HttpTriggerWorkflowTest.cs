using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Toast.Common.Builders;
using Toast.Common.Configurations;
using Toast.Common.Exceptions;
using Toast.Common.Models;
using Toast.Sms.Configurations;
using Toast.Sms.Workflows;
using Toast.Tests.Common.Fakes;

namespace Toast.Sms.Tests.Workflows
{
    [TestClass]
    public class HttpTriggerWorkflowTests{
        private Mock<IHttpClientFactory> _factory;
        private Mock<MediaTypeFormatter> _fomatter;

        [TestInitialize]
        public void Init()
        {
            this._factory = new Mock<IHttpClientFactory>();
            this._fomatter = new Mock<MediaTypeFormatter>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            this._factory = null;
            this._fomatter = null;
        }
        [TestMethod]
        public void Given_Type_When_Initiated_Then_It_Should_Implement_Interface()
        {
            var workflow = new HttpTriggerWorkflow(this._factory.Object, this._fomatter.Object);

            var hasInterface = workflow.GetType().HasInterface<IHttpTriggerWorkflow>();

            hasInterface.Should().BeTrue();
        }

        [TestMethod]
        public async Task Given_NullHeader_When_Invoke_ValidateHeaderAsync_Then_It_Should_Throw_NullReferenceException()
        {
            var req = new Mock<HttpRequest>();
            var workflow = new HttpTriggerWorkflow(this._factory.Object, this._fomatter.Object);

            Func<Task> func = async () => await workflow.ValidateHeaderAsync(req.Object);

            await func.Should().ThrowAsync<NullReferenceException>();
        }

        [TestMethod]
        public async Task Given_NoHeader_When_Invoke_ValidateHeaderAsync_Then_It_Should_Throw_InvalidOperationException()
        {
            var headers = new HeaderDictionary();
            headers.Add("Authorization", "Basic");

            var req = new Mock<HttpRequest>();
            req.SetupGet(p => p.Headers).Returns(headers);

            var workflow = new HttpTriggerWorkflow(this._factory.Object, this._fomatter.Object);

            Func<Task> func = async () => await workflow.ValidateHeaderAsync(req.Object);

            await func.Should().ThrowAsync<InvalidOperationException>();

        }

        [DataTestMethod]
        [DataRow("hello", " ")]
        [DataRow(" ", "world")]
        public async Task Given_InvalidHeader_When_Invoke_ValidateHeaderAsync_Then_It_Should_Throw_Exception(string username, string password)
        {
            var bytes = Encoding.UTF8.GetBytes($"{username}:{password}");
            var encoded = Convert.ToBase64String(bytes);

            var headers = new HeaderDictionary();
            headers.Add("Authorization", $"Basic {encoded}");

            var req = new Mock<HttpRequest>();
            req.SetupGet(p => p.Headers).Returns(headers);

            var workflow = new HttpTriggerWorkflow(this._factory.Object, this._fomatter.Object);

            Func<Task> func = async () => await workflow.ValidateHeaderAsync(req.Object);

            await func.Should().ThrowAsync<RequestHeaderNotValidException>();
        }

        [DataTestMethod]
        [DataRow("hello", "world")]
        public async Task Given_ValidHeader_When_Invoke_ValidateHeaderAsync_Then_It_Should_Return_Result(string username, string password)
        {
            var bytes = Encoding.UTF8.GetBytes($"{username}:{password}");
            var encoded = Convert.ToBase64String(bytes);

            var headers = new HeaderDictionary();
            headers.Add("Authorization", $"Basic {encoded}");

            var req = new Mock<HttpRequest>();
            req.SetupGet(p => p.Headers).Returns(headers);

            var workflow = new HttpTriggerWorkflow(this._factory.Object, this._fomatter.Object);

            var result = await workflow.ValidateHeaderAsync(req.Object);

            result.Should().BeOfType<HttpTriggerWorkflow>();
        }

        [DataTestMethod]
        [DataRow("hello", "world")]
        public async Task Given_ValidHeader_When_Invoke_ValidateHeaderAsync_Then_It_Should_Contain_Headers(string username, string password)
        {
            var bytes = Encoding.UTF8.GetBytes($"{username}:{password}");
            var encoded = Convert.ToBase64String(bytes);

            var headers = new HeaderDictionary();
            headers.Add("Authorization", $"Basic {encoded}");

            var req = new Mock<HttpRequest>();
            req.SetupGet(p => p.Headers).Returns(headers);

            var workflow = new HttpTriggerWorkflow(this._factory.Object, this._fomatter.Object);

            var result = await workflow.ValidateHeaderAsync(req.Object);
            var fi = workflow.GetType().GetField("_headers", BindingFlags.NonPublic | BindingFlags.Instance);
            var field = fi.GetValue(result) as RequestHeaderModel;

            field.Should().NotBeNull();
            field.AppKey.Should().Be(username);
            field.SecretKey.Should().Be(password);
        }

        [TestMethod]
        public async Task Given_NullSettings_When_Invoke_BuildRequestUrlAync_Then_It_Should_Throw_Exception()
        { 
            var workflow = new HttpTriggerWorkflow(this._factory.Object, this._fomatter.Object);

            Func<Task> func = async () => await workflow.BuildRequestUrlAsync("Test", null);

            await func.Should().ThrowAsync<InvalidOperationException>();
        }

        [TestMethod]
        public async Task Given_NoSettings_When_Invoke_BuildRequestUrlAync_Then_It_Should_Throw_Exception()
        { 
            var set = new Mock<ToastSettings<SmsEndpointSettings>>();
            var workflow = new HttpTriggerWorkflow(this._factory.Object, this._fomatter.Object);

            Func<Task> func = async () => await workflow.BuildRequestUrlAsync("Test", set.Object);

            await func.Should().ThrowAsync<InvalidOperationException>();
        }

        [TestMethod]
        public void Given_nullHeader_And_nullQueries_When_Invoke_BuildRequestUrlAsync_Then_It_Return_null()
        {
            var workflow = new HttpTriggerWorkflow(this._factory.Object, this._fomatter.Object);
            var settings = new FakeEndpointSettings()
            {
                BaseUrl = "http://localhost:7071/api/{version}/appKeys/{appKey}",
                Version = "v3.0"

            };

            workflow.BuildRequestUrlAsync("HttpTrigger", settings);
            var fi = workflow.GetType().GetField("_requestUrl", BindingFlags.NonPublic | BindingFlags.Instance);
            var field = fi.GetValue(workflow);

            field.Should().Be(null);
            
        }

        [TestMethod]
        public void Given_ValidSettings_When_Invoke_BuildRequestUrlAsync_Then_It_Return_requestUrl()
        {
            var workflow = new HttpTriggerWorkflow(this._factory.Object, this._fomatter.Object);
            var settings = new FakeEndpointSettings()
            {
                BaseUrl = "http://localhost:7071/api/{version}/appKeys/{appKey}",
                Version = "v3.0"

            };

            var header = new RequestHeaderModel() { AppKey = "hello", SecretKey = "world" };
            var headers = typeof(HttpTriggerWorkflow).GetField("_headers", BindingFlags.Instance | BindingFlags.NonPublic);
            headers.SetValue(workflow, header);

            var query = new FakeRequestQueries() {};
            var queries = typeof(HttpTriggerWorkflow).GetField("_queries", BindingFlags.Instance | BindingFlags.NonPublic);
            queries.SetValue(workflow, query);

            workflow.BuildRequestUrlAsync("HttpTrigger", settings);
            var fi = workflow.GetType().GetField("_requestUrl", BindingFlags.NonPublic | BindingFlags.Instance);
            var field = fi.GetValue(workflow);

            field.Should().Be("http://localhost:7071/api/v3.0/appKeys/hello/HttpTrigger");
        }
    }
}