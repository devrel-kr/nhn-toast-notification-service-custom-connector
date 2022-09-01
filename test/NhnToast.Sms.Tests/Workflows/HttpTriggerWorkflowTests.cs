using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using Toast.Common.Exceptions;
using Toast.Common.Models;
using Toast.Sms.Workflows;

namespace Toast.Sms.Tests.Workflows
{
    [TestClass]
    public class HttpTriggerWorkflowTests
    {
        [TestMethod]
        public void Given_Type_When_Initiated_Then_It_Should_Implement_Interface()
        {
            var workflow = new HttpTriggerWorkflow();

            var hasInterface = workflow.GetType().HasInterface<IHttpTriggerWorkflow>();

            hasInterface.Should().BeTrue();
        }

        [TestMethod]
        public void Given_NullHeader_When_Invoke_ValidateHeaderAsync_Then_It_Should_Throw_Exception()
        {
            var req = new Mock<HttpRequest>();
            var workflow = new HttpTriggerWorkflow();

            Func<Task> func = async () => await workflow.ValidateHeaderAsync(req.Object);

            func.Should().ThrowAsync<RequestHeaderNotValidException>();
        }

        [TestMethod]
        public void Given_NoHeader_When_Invoke_ValidateHeaderAsync_Then_It_Should_Throw_Exception()
        {
            var headers = new HeaderDictionary();
            headers.Add("Authorization", "Basic");

            var req = new Mock<HttpRequest>();
            req.SetupGet(p => p.Headers).Returns(headers);

            var workflow = new HttpTriggerWorkflow();

            Func<Task> func = async () => await workflow.ValidateHeaderAsync(req.Object);

            func.Should().ThrowAsync<RequestHeaderNotValidException>();
        }

        [DataTestMethod]
        [DataRow("hello", null)]
        [DataRow(null, "world")]
        public void Given_InvalidHeader_When_Invoke_ValidateHeaderAsync_Then_It_Should_Throw_Exception(string username, string password)
        {
            var bytes = Encoding.UTF8.GetBytes($"{username}:{password}");
            var encoded = Convert.ToBase64String(bytes);

            var headers = new HeaderDictionary();
            headers.Add("Authorization", $"Basic {encoded}");

            var req = new Mock<HttpRequest>();
            req.SetupGet(p => p.Headers).Returns(headers);

            var workflow = new HttpTriggerWorkflow();

            Func<Task> func = async () => await workflow.ValidateHeaderAsync(req.Object);

            func.Should().ThrowAsync<RequestHeaderNotValidException>();
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

            var workflow = new HttpTriggerWorkflow();

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

            var workflow = new HttpTriggerWorkflow();

            var result = await workflow.ValidateHeaderAsync(req.Object);
            var fi = workflow.GetType().GetField("_headers", BindingFlags.NonPublic | BindingFlags.Instance);
            var field = fi.GetValue(result) as RequestHeaderModel;

            field.Should().NotBeNull();
            field.AppKey.Should().Be(username);
            field.SecretKey.Should().Be(password);
        }
    }
}