using System;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Toast.Common.Exceptions;
using Toast.Sms.Models;
using Toast.Sms.Validators;

namespace Toast.Sms.Tests.Validators
{
    [TestClass]
    public class SendMessagesRequestBodyValidatorTests
    {
        [DataTestMethod]

        public void Given_Values_When_Validate_Invoked_Then_It_Should_Return_Result(string templateId, string sendNo, string recipientNo, bool expected)
        {
            var payloads = new SendMessagesRequestBody()
            {
                TemplateId = templateId,
                SenderNumber = sendNo
            };
            var validator = new SendMessagesRequestBodyValidator();

            var result = validator.Validate(payloads);

            result.IsValid.Should().Be(expected);
        }

        [DataTestMethod]

        public void Given_InvalidValues_When_Validate_Invoked_Then_It_Should_Throw_Exception(string templateId, string sendNo, string recipientNo)
        {
            var payloads = new SendMessagesRequestBody()
            {
                TemplateId = templateId,
                SenderNumber = sendNo
            };
            var validator = new SendMessagesRequestBodyValidator();

            Func<Task> func = async () => await SendMessagesRequestBodyValidatorExtension.Validate(Task.FromResult(payloads), validator).ConfigureAwait(false);

            func.Should().ThrowAsync<RequestBodyNotValidException>();
        }

        [DataTestMethod]

        public async Task Given_ValidValues_When_Validate_Invoked_Then_It_Should_Return_Result(string templateId, string sendNo, string recipientNo)
        {
            var payloads = new SendMessagesRequestBody()
            {
                TemplateId = templateId,
                SenderNumber = sendNo
            };
            var validator = new SendMessagesRequestBodyValidator();

            var result = await SendMessagesRequestBodyValidatorExtension.Validate(Task.FromResult(payloads), validator).ConfigureAwait(false);

            result.Should().NotBeNull();
            result.Should().BeOfType<SendMessagesRequestBody>();
        }
    }
}