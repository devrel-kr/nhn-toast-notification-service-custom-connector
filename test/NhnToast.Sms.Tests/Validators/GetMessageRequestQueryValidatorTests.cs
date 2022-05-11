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
    public class GetMessageRequestQueryValidatorTests
    {
        [DataTestMethod]
        [DataRow(-1,false)]
        [DataRow(0, false)]
        [DataRow(1,true)]
        public void Given_Values_When_Validate_Invoked_Then_It_Should_Return_Result(int recipientSequenceNumber, bool expected)
        {
            var queries = new GetMessageRequestQueries()
            {
                RecipientSequenceNumber = recipientSequenceNumber
            };
            var validator = new GetMessageRequestQueryValidator();

            var result = validator.Validate(queries);

            result.IsValid.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        public void Given_InvalidValues_When_Validate_Invoked_Then_It_Should_Throw_Exception(int recipientSequenceNumber)
        {
            var queries = new GetMessageRequestQueries()
            {
                RecipientSequenceNumber = recipientSequenceNumber
            };
            var validator = new GetMessageRequestQueryValidator();

            Func<Task> func = async () => await GetMessageRequestQueryValidatorExtension.Validate(Task.FromResult(queries), validator).ConfigureAwait(false);

            func.Should().ThrowAsync<RequestQueryNotValidException>();
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(2)]
        public async Task Given_ValidValues_When_Validate_Invoked_Then_It_Should_Return_Result(int recipientSequenceNumber)
        {
            var queries = new GetMessageRequestQueries()
            {
                RecipientSequenceNumber = recipientSequenceNumber
            };
            var validator = new GetMessageRequestQueryValidator();

            var result = await GetMessageRequestQueryValidatorExtension.Validate(Task.FromResult(queries), validator).ConfigureAwait(false);

            result.Should().NotBeNull();
            result.Should().BeOfType<GetMessageRequestQueries>();
        }
    }
}