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
    public class ListMessageStatusRequestQueryValidatorTests
    {
        [DataTestMethod]
        [DataRow("2022-05-01 00:00:00", "2022-05-01 23:59:59", null, null, null, true)]
        [DataRow("2022-05-01 00:00:00", null, null, null, null, false)]
        [DataRow(null, "2022-05-01 23:59:59", null, null, null, false)]
        [DataRow("2022-05-01 00:00:00", "2022-05-01 23:59:59", null, 0, null, false)]
        [DataRow("2022-05-01 00:00:00", "2022-05-01 23:59:59", null, null, 0, false)]

        public void Given_Values_When_Validate_Invoked_Then_It_Should_Return_Result(string startUpdateDate, string endUpdateDate, string msgType, int? pageNumber, int? pageSize, bool expected)
        {
            var queries = new ListMessageStatusRequestQuries()
            {
                StartUpdateDate = startUpdateDate,
                EndUpdateDate = endUpdateDate,
                MessageType = msgType,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            var validator = new ListMessageStatusRequestQueryValidator();

            var result = validator.Validate(queries);

            result.IsValid.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow("2022-05-01 00:00:00", null, null, null, null)]
        [DataRow(null, "2022-05-01 23:59:59", null, null, null)]
        [DataRow("2022-05-01 00:00:00", "2022-05-01 23:59:59", null, 0, null)]
        [DataRow("2022-05-01 00:00:00", "2022-05-01 23:59:59", null, null, 0)]

        public void Given_InvalidValues_When_Validate_Invoked_Then_It_Should_Throw_Exception(string startUpdateDate, string endUpdateDate, string msgType, int? pageNumber, int? pageSize)
        {
            var queries = new ListMessageStatusRequestQuries()
            {
                StartUpdateDate = startUpdateDate,
                EndUpdateDate = endUpdateDate,
                MessageType = msgType,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            var validator = new ListMessageStatusRequestQueryValidator();

            Func<Task> func = async () => await ListMessageStatusRequestQueryValidatorExtension.Validate(Task.FromResult(queries), validator).ConfigureAwait(false);

            func.Should().ThrowAsync<RequestQueryNotValidException>();
        }

        [DataTestMethod]
        [DataRow("2022-05-01 00:00:00", "2022-05-01 23:59:59", null, null, null, 1, 15)]
        [DataRow("2022-05-01 00:00:00", "2022-05-01 23:59:59", null, 2, 10, 2, 10)]

        public async Task Given_ValidValues_When_Validate_Invoked_Then_It_Should_Return_Result(string startUpdateDate, string endUpdateDate, string msgType, int? pageNumber, int? pageSize, int expectedPageNumber, int expectedPageSize)
        {
            var queries = new ListMessageStatusRequestQuries()
            {
                StartUpdateDate = startUpdateDate,
                EndUpdateDate = endUpdateDate,
                MessageType = msgType,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            var validator = new ListMessageStatusRequestQueryValidator();

            var result = await ListMessageStatusRequestQueryValidatorExtension.Validate(Task.FromResult(queries), validator).ConfigureAwait(false);

            result.Should().NotBeNull();
            result.Should().BeOfType<ListMessageStatusRequestQuries>();
            result.StartUpdateDate.Should().Be(startUpdateDate);
            result.EndUpdateDate.Should().Be(endUpdateDate);
            result.MessageType.Should().Be(msgType);
            result.PageNumber.Should().Be(expectedPageNumber);
            result.PageSize.Should().Be(expectedPageSize);
        }
    }
}