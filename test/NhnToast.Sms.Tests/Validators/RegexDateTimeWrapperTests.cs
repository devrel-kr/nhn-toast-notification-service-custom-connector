using System;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Toast.Sms.Validators;

namespace Toast.Sms.Tests.Validators
{
    [TestClass]
    public class RegexDateTimeWrapperTests
    {
        [DataTestMethod]
        [DataRow("2022-05-01 00:00:00", true)]
        [DataRow("0111-09-01 00:00:00", true)]
        [DataRow("9999-09-01 00:00:00", true)]
        [DataRow("9999-15-41 00:00:00", false)]
        [DataRow("20220501 00:00:00", false)]
        [DataRow("2022-08-90 55:00:00", false)]
        [DataRow("2022-0&-OO 00:oo:OT", false)]
        public void Given_Values_When_Validate_Invoked_Then_It_Should_Return_Result(string date, bool expected)
        {
            //Arrange
            var wapper = new RegexDateTimeWrapper();

            //Act
            var result = wapper.IsMatch(date);

            //Assert
            result.Should().Be(expected);

        }

    }
}