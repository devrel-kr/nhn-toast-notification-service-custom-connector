using System;
using System.Net;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Toast.Common.Exceptions;
using Toast.Common.Models;
using Toast.Common.Validators;

namespace Toast.Common.Tests.Validators
{
    [TestClass]
    public class RequestHeaderValidatorTests
    {
        [DataTestMethod]
        [DataRow(null, null, "Not Found", HttpStatusCode.NotFound)]
        [DataRow("hello", null, "Unauthorized", HttpStatusCode.Unauthorized)]
        [DataRow(null, "world", "Not Found", HttpStatusCode.NotFound)]
        public void Given_InvalidHeader_When_Validator_Invoked_Then_It_Should_Throw_Exception(string appKey, string secretKey, string message, HttpStatusCode statusCode)
        {
            var headers = new RequestHeaderModel() { AppKey = appKey, SecretKey = secretKey };

            Func<Task> func = async () => await RequestHeaderValidator.Validate(Task.FromResult(headers)).ConfigureAwait(false);

            var ex = func.Should().ThrowAsync<RequestHeaderNotValidException>().Result.Which;
            ex.Message.Should().Be(message);
            ex.StatusCode.Should().Be(statusCode);
        }

        [DataTestMethod]
        [DataRow("hello", "world")]
        public async Task Given_Header_When_Validator_Invoked_Then_It_Should_Return_Result(string appKey, string secretKey)
        {
            var headers = new RequestHeaderModel() { AppKey = appKey, SecretKey = secretKey };

            var result = await RequestHeaderValidator.Validate(Task.FromResult(headers)).ConfigureAwait(false);

            result.AppKey.Should().Be(appKey);
            result.SecretKey.Should().Be(secretKey);
        }
    }
}