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
    public class ListMessagesRequestQueryValidatorTests
    {
        [DataTestMethod]
        [DataRow(null, null, null, "2022-05-10 00:00:00", "2022-05-11 00:00:00", null, null, null, null, null, null, null, null, null, null, null, null, true)]
        [DataRow(null, "2022-05-10 00:00:00", "2022-05-11 00:00:00", null, null, null, null, null, null, null, null, null, null, null, null, null, null, true)]
        [DataRow("1234567890", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, true)]
        [DataRow("1234567890", null, null, null, null, null, null, null, null, null, "2", "MTR1", "MTR2_1", null, null, null, null, true)]
        [DataRow("1234567890", null, null, null, null, "2022-05-10 00:00:00", "2022-05-11 00:00:00", null, null, null, null, null, null, null, null, null, null, true)]
        [DataRow(null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, false)]
        [DataRow(null, null, null, "2022-05-10 00:00:00", null, null, null, null, null, null, null, null, null, null, null, null, null, false)]
        [DataRow(null, null, null, null, "2022-05-11 00:00:00", null, null, null, null, null, null, null, null, null, null, null, null, false)]
        [DataRow(null, "2022-05-10 00:00:00", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, false)]
        [DataRow(null, null, "2022-05-11 00:00:00", null, null, null, null, null, null, null, null, null, null, null, null, null, null, false)]
        [DataRow(null, null, null, "20220510000000", "20220511000000", null, null, null, null, null, null, null, null, null, null, null, null, false)]
        [DataRow(null, "20220510000000", "20220511000000", null, null, null, null, null, null, null, null, null, null, null, null, null, null, false)]
        [DataRow(null, null, null, "2022-05-11 00:00:00", "2022-05-10 00:00:00", null, null, null, null, null, null, null, null, null, null, null, null, false)]
        [DataRow(null, "2022-05-11 00:00:00", "2022-05-10 00:00:00", null, null, null, null, null, null, null, null, null, null, null, null, null, null, false)]
        [DataRow("1234567890", null, null, null, null, "20220510000000", "20220511000000", null, null, null, null, null, null, null, null, null, null, false)]
        [DataRow("1234567890", null, null, null, null, "2022-05-11 00:00:00", "2022-05-10 00:00:00", null, null, null, null, null, null, null, null, null, null, false)]

        public void Given_Values_When_Validate_Invoked_Then_It_Should_Return_Result(string requestId, string startRequestDate, string endRequestDate, string startCreateDate, string endCreateDate, 
            string startResultDate, string endResultDate, string sendNumber, string recipientNumber, string templateId, string msgStatus, string resultCode, string subResultCode, 
            string SenderGroupingKey, string recipientGroupingkey, int? pageNum, int? pageSize, bool expected)
        {
            var queries = new ListMessagesRequestQueries()
            {
                RequestId = requestId,
                StartRequestDate = startRequestDate,
                EndRequestDate = endRequestDate,
                StartCreateDate = startCreateDate,
                EndCreateDate = endCreateDate,
                StartResultDate = startResultDate,
                EndResultDate = endResultDate,
                SendNumber = sendNumber,
                RecipientNumber = recipientNumber,
                TemplateId = templateId,
                MessageStatus = msgStatus,
                ResultCode = resultCode,
                SubResultCode = subResultCode,
                SenderGroupingKey = SenderGroupingKey,
                RecipientGroupingKey = recipientGroupingkey,
                PageNumber = pageNum,
                PageSize = pageSize
            };
            var validator = new ListMessagesRequestQueryValidator();

            var result = validator.Validate(queries);

            result.IsValid.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null)]
        [DataRow(null, "2022-05-10 00:00:00", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null)]
        [DataRow(null, null, null, "2022-05-10 00:00:00", null, null, null, null, null, null, null, null, null, null, null, null, null)]
        [DataRow(null, null, null, null, "2022-05-10 00:00:00", null, null, null, null, null, null, null, null, null, null, null, null)]
        [DataRow(null, null, "2022-05-10 00:00:00", null, null, null, null, null, null, null, null, null, null, null, null, null, null)]
        [DataRow(null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null)]
        [DataRow(null, null, null, "2022-05-10 00:00:00", null, null, null, null, null, null, null, null, null, null, null, null, null)]
        [DataRow(null, null, null, null, "2022-05-11 00:00:00", null, null, null, null, null, null, null, null, null, null, null, null)]
        [DataRow(null, "2022-05-10 00:00:00", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null)]
        [DataRow(null, null, "2022-05-11 00:00:00", null, null, null, null, null, null, null, null, null, null, null, null, null, null)]
        [DataRow(null, null, null, "20220510000000", "20220511000000", null, null, null, null, null, null, null, null, null, null, null, null)]
        [DataRow(null, "20220510000000", "20220511000000", null, null, null, null, null, null, null, null, null, null, null, null, null, null)]
        [DataRow(null, null, null, "2022-05-11 00:00:00", "2022-05-10 00:00:00", null, null, null, null, null, null, null, null, null, null, null, null)]
        [DataRow(null, "2022-05-11 00:00:00", "2022-05-10 00:00:00", null, null, null, null, null, null, null, null, null, null, null, null, null, null)]
        [DataRow("1234567890", null, null, null, null, "20220510000000", "20220511000000", null, null, null, null, null, null, null, null, null, null)]
        [DataRow("1234567890", null, null, null, null, "2022-05-11 00:00:00", "2022-05-10 00:00:00", null, null, null, null, null, null, null, null, null, null)]

        public void Given_InvalidValues_When_Validate_Invoked_Then_It_Should_Throw_Exception(string requestId, string startRequestDate, string endRequestDate, string startCreateDate, string endCreateDate, 
            string startResultDate, string endResultDate, string sendNumber, string recipientNumber, string templateId, string msgStatus, string resultCode, string subResultCode, 
            string SenderGroupingKey, string recipientGroupingkey, int? pageNum, int? pageSize)
        {
            var queries = new ListMessagesRequestQueries()
            {
                RequestId = requestId,
                StartRequestDate = startRequestDate,
                EndRequestDate = endRequestDate,
                StartCreateDate = startCreateDate,
                EndCreateDate = endCreateDate,
                StartResultDate = startResultDate,
                EndResultDate = endResultDate,
                SendNumber = sendNumber,
                RecipientNumber = recipientNumber,
                TemplateId = templateId,
                MessageStatus = msgStatus,
                ResultCode = resultCode,
                SubResultCode = subResultCode,
                SenderGroupingKey = SenderGroupingKey,
                RecipientGroupingKey = recipientGroupingkey,
                PageNumber = pageNum,
                PageSize = pageSize
            };
            var validator = new ListMessagesRequestQueryValidator();

            Func<Task> func = async () => await ListMessagesQueryValidatorExtension.Validate(Task.FromResult(queries), validator).ConfigureAwait(false);

            func.Should().ThrowAsync<RequestQueryNotValidException>();
        }

        [DataTestMethod]
        [DataRow("1234567890", "2022-05-10 00:00:00", "2022-05-11 00:00:00", null, null, null, null, null, null, null, null, null, null, null, null, 2, 10, 2, 10)]
        [DataRow("1234567890", "2022-05-10 00:00:00", "2022-05-11 00:00:00", null, null, null, null, null, null, null, null, null, null, null, null, null, null, 1, 15)]
        [DataRow("1234567890", "2022-05-10 00:00:00", "2022-05-11 00:00:00", "2022-05-10 00:00:00", "2022-05-11 00:00:00", "2022-05-10 00:00:00", "2022-05-11 00:00:00", "1234567890", "1234567890", "1234567890", "2", "MTR1", "MTR2_1", "1234567890", "1234567890", null, null, 1, 15)]

        public async Task Given_ValidValues_When_Validate_Invoked_Then_It_Should_Return_Result(string requestId, string startRequestDate, string endRequestDate, string startCreateDate, string endCreateDate, 
            string startResultDate, string endResultDate, string sendNumber, string recipientNumber, string templateId, string msgStatus, string resultCode, string subResultCode, 
            string SenderGroupingKey, string recipientGroupingkey, int? pageNum, int? pageSize, int expectedPageNumber, int expectedPageSize)
        {
            var queries = new ListMessagesRequestQueries()
            {
                RequestId = requestId,
                StartRequestDate = startRequestDate,
                EndRequestDate = endRequestDate,
                StartCreateDate = startCreateDate,
                EndCreateDate = endCreateDate,
                StartResultDate = startResultDate,
                EndResultDate = endResultDate,
                SendNumber = sendNumber,
                RecipientNumber = recipientNumber,
                TemplateId = templateId,
                MessageStatus = msgStatus,
                ResultCode = resultCode,
                SubResultCode = subResultCode,
                SenderGroupingKey = SenderGroupingKey,
                RecipientGroupingKey = recipientGroupingkey,
                PageNumber = pageNum,
                PageSize = pageSize
            };
            var validator = new ListMessagesRequestQueryValidator();

            var result = await ListMessagesQueryValidatorExtension.Validate(Task.FromResult(queries), validator).ConfigureAwait(false);

            result.Should().NotBeNull();
            result.Should().BeOfType<ListMessagesRequestQueries>();
            result.RequestId.Should().Be(requestId);
            result.StartRequestDate.Should().Be(startRequestDate);
            result.EndRequestDate.Should().Be(endRequestDate);
            result.StartCreateDate.Should().Be(startCreateDate);
            result.EndCreateDate.Should().Be(endCreateDate);
            result.StartResultDate.Should().Be(startResultDate);
            result.EndResultDate.Should().Be(endResultDate);
            result.SendNumber.Should().Be(sendNumber);
            result.RecipientNumber.Should().Be(recipientNumber);
            result.TemplateId.Should().Be(templateId);
            result.MessageStatus.Should().Be(msgStatus);
            result.ResultCode.Should().Be(resultCode);
            result.SubResultCode.Should().Be(subResultCode);
            result.SenderGroupingKey.Should().Be(SenderGroupingKey);
            result.RecipientGroupingKey.Should().Be(recipientGroupingkey);
            result.PageNumber.Should().Be(expectedPageNumber);
            result.PageSize.Should().Be(expectedPageSize);
        }
    }
}