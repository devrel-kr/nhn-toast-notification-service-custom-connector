using System;
using System.Collections.Generic;
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
        [DataRow(null, "Hello world", "1234567890", "2022-05-10 00:00:00", null, "0987654321", null, null, null, null, null, true)]
        [DataRow(null, "Hello world", "1234567890", "2022-05-10 00:00:00", null, null, null, null, null, null, null, false)]
        [DataRow(null, "Hello world", null, "2022-05-10 00:00:00", null, "0987654321", null, null, null, null, null, false)]
        [DataRow(null, null, "1234567890", "2022-05-10 00:00:00", null, "0987654321", null, null, null, null, null, false)]
        [DataRow(null, "Hello world", "1234567890", "2022051000000", null, "0987654321", null, null, null, null, null, false)]
        [DataRow(null, "Hello world", "12345678901234567890", "2022-05-10 00:00:00", null, "0987654321", null, null, null, null, null, false)]
        [DataRow(null, "Hello world", "1234567890", "2022-05-10 00:00:00", null, "098765432109876543210987654321", null, null, null, null, null, false)]
        public void Given_Values_When_Validate_Invoked_Then_It_Should_Return_Result(string templateId, string body, string sendNo, string requestDate, string senderGroupingKey, 
            string recipientNo, string countryCode, string InternationalRecipientNo, string recipientGroupingKey, string userId, string statsId, bool expected)
        {
            var recipient = new SendMessagesRequestRecipient()
            {
                RecipientNumber = recipientNo,
                CountryCode = countryCode,
                InternationalRecipientNumber = InternationalRecipientNo,
                RecipientGroupingKey = recipientGroupingKey
            };

            List<SendMessagesRequestRecipient> recipientlist = new List<SendMessagesRequestRecipient>();
            recipientlist.Add(recipient);

            var payloads = new SendMessagesRequestBody()
            {
                TemplateId = templateId,
                Body = body,
                SenderNumber = sendNo,
                RequestDate = requestDate,
                SenderGroupingKey = senderGroupingKey,
                Recipients = recipientlist,
                UserId = userId,
                StatsId = statsId
            };
            var validator = new SendMessagesRequestBodyValidator();

            var result = validator.Validate(payloads);

            result.IsValid.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(null, "Hello world", "1234567890", "2022-05-10 00:00:00", null, null, null, null, null, null, null)]
        [DataRow(null, "Hello world", null, "2022-05-10 00:00:00", null, "0987654321", null, null, null, null, null)]
        [DataRow(null, null, "1234567890", "2022-05-10 00:00:00", null, "0987654321", null, null, null, null, null)]
        [DataRow(null, "Hello world", "1234567890", "2022051000000", null, "0987654321", null, null, null, null, null)]
        [DataRow(null, "Hello world", "12345678901234567890", "2022-05-10 00:00:00", null, "0987654321", null, null, null, null, null)]
        [DataRow(null, "Hello world", "1234567890", "2022-05-10 00:00:00", null, "098765432109876543210987654321", null, null, null, null, null)]

        public void Given_InvalidValues_When_Validate_Invoked_Then_It_Should_Throw_Exception(string templateId, string body, string sendNo, string requestDate, string senderGroupingKey,
            string recipientNo, string countryCode, string InternationalRecipientNo, string recipientGroupingKey, string userId, string statsId)
        {
            var recipient = new SendMessagesRequestRecipient()
            {
                RecipientNumber = recipientNo,
                CountryCode = countryCode,
                InternationalRecipientNumber = InternationalRecipientNo,
                RecipientGroupingKey = recipientGroupingKey
            };

            List<SendMessagesRequestRecipient> recipientlist = new List<SendMessagesRequestRecipient>();
            recipientlist.Add(recipient);

            var payloads = new SendMessagesRequestBody()
            {
                TemplateId = templateId,
                Body = body,
                SenderNumber = sendNo,
                RequestDate = requestDate,
                SenderGroupingKey = senderGroupingKey,
                Recipients = recipientlist,
                UserId = userId,
                StatsId = statsId
            };
            var validator = new SendMessagesRequestBodyValidator();

            Func<Task> func = async () => await SendMessagesRequestBodyValidatorExtension.Validate(Task.FromResult(payloads), validator).ConfigureAwait(false);

            func.Should().ThrowAsync<RequestBodyNotValidException>();
        }

        [DataTestMethod]
        [DataRow(null, "Hello world", "1234567890", "2022-05-10 00:00:00", null, "0987654321", null, null, null, null, null)]
        [DataRow("1234567", "Hello world", "1234567890", "2022-05-10 00:00:00", null, "0987654321", null, null, null, null, null)]
        [DataRow(null, "Hello world", "1234567890", "2022-05-10 00:00:00", "11111", "0987654321", null, null, null, null, null)]
        [DataRow(null, "Hello world", "1234567890", "2022-05-10 00:00:00", "11111", "0987654321", null, null, null, "2222", null)]
        [DataRow(null, "Hello world", "1234567890", "2022-05-10 00:00:00", "11111", "0987654321", null, null, null, null, "3333")]
        public async Task Given_ValidValues_When_Validate_Invoked_Then_It_Should_Return_Result(string templateId, string body, string sendNo, string requestDate, string senderGroupingKey,
            string recipientNo, string countryCode, string InternationalRecipientNo, string recipientGroupingKey, string userId, string statsId)
        {
            var recipient = new SendMessagesRequestRecipient()
            {
                RecipientNumber = recipientNo,
                CountryCode = countryCode,
                InternationalRecipientNumber = InternationalRecipientNo,
                RecipientGroupingKey = recipientGroupingKey
            };

            List<SendMessagesRequestRecipient> recipientlist = new List<SendMessagesRequestRecipient>();
            recipientlist.Add(recipient);

            var payloads = new SendMessagesRequestBody()
            {
                TemplateId = templateId,
                Body = body,
                SenderNumber = sendNo,
                RequestDate = requestDate,
                SenderGroupingKey = senderGroupingKey,
                Recipients = recipientlist,
                UserId = userId,
                StatsId = statsId
            };
            var validator = new SendMessagesRequestBodyValidator();

            var result = await SendMessagesRequestBodyValidatorExtension.Validate(Task.FromResult(payloads), validator).ConfigureAwait(false);

            result.Should().NotBeNull();
            result.Should().BeOfType<SendMessagesRequestBody>();
            result.TemplateId.Should().Be(templateId);
            result.Body.Should().Be(body);
            result.SenderNumber.Should().Be(sendNo);
            result.RequestDate.Should().Be(requestDate);
            result.SenderGroupingKey.Should().Be(senderGroupingKey);
            result.UserId.Should().Be(userId);
            result.StatsId.Should().Be(statsId);
        }
    }
}