using System;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Toast.Sms.Verification.Models;
using Toast.Sms.Verification.Validators;

namespace Toast.Sms.Verification.Tests.Validators
{
    [TestClass]
    public class ListSendersRequestQueryValidatorTests
    {
        [DataTestMethod]
        [DataRow("T", "N", null, null, false)]
        [DataRow("Y", "F", null, null, false)]
        [DataRow("Y", "N", 0, null, false)]
        [DataRow("Y", "N", null, 0, false)]
        [DataRow("Y", "N", null, null, true)]
        [DataRow("N", "Y", null, null, true)]
        public void Given_Values_When_Validate_Invoked_Then_It_Should_Return_Result(string useNumber, string blockedNumber, int? pageNumber, int? pageSize, bool expected)
        {
            var queries = new ListSendersRequestQueries()
            {
                SenderNumber = "1234567890",
                UseNumber = useNumber,
                BlockedNumber = blockedNumber,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            var validator = new ListSendersRequestQueryValidator();

            var result = validator.Validate(queries);

            result.IsValid.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow("T", "N", null, null)]
        [DataRow("Y", "F", null, null)]
        [DataRow("Y", "N", 0, null)]
        [DataRow("Y", "N", null, 0)]
        public void Given_InvalidValues_When_Validate_Invoked_Then_It_Should_Throw_Exception(string useNumber, string blockedNumber, int? pageNumber, int? pageSize)
        {
            var queries = new ListSendersRequestQueries()
            {
                SenderNumber = "1234567890",
                UseNumber = useNumber,
                BlockedNumber = blockedNumber,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            var validator = new ListSendersRequestQueryValidator();

            Func<Task> func = async () => await ListSendersRequestQueryValidatorExtension.Validate(Task.FromResult(queries), validator).ConfigureAwait(false);

            func.Should().ThrowAsync<InvalidOperationException>();
        }

        [DataTestMethod]
        [DataRow(null, null, null, null, 1, 15)]
        [DataRow("Y", "N", null, null, 1, 15)]
        [DataRow("N", "Y", 2, 10, 2, 10)]
        public async Task Given_ValidValues_When_Validate_Invoked_Then_It_Should_Return_Result(string useNumber, string blockedNumber, int? pageNumber, int? pageSize, int expectedPageNumber, int expectedPageSize)
        {
            var queries = new ListSendersRequestQueries()
            {
                SenderNumber = "1234567890",
                UseNumber = useNumber,
                BlockedNumber = blockedNumber,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            var validator = new ListSendersRequestQueryValidator();

            var result = await ListSendersRequestQueryValidatorExtension.Validate(Task.FromResult(queries), validator).ConfigureAwait(false);

            result.Should().NotBeNull();
            result.Should().BeOfType<ListSendersRequestQueries>();
            result.UseNumber.Should().Be(useNumber);
            result.BlockedNumber.Should().Be(blockedNumber);
            result.PageNumber.Should().Be(expectedPageNumber);
            result.PageSize.Should().Be(expectedPageSize);
        }
    }
}