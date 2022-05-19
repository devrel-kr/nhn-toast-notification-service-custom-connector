using System.Collections.Generic;
using System.Linq;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

using Toast.Common.Models;
using Toast.Sms.Models;

namespace Toast.Sms.Tests.Models
{
    [TestClass]
    public class ListMessagesResponseModelTests
    {
        private static JsonSerializerSettings serializerSettings = new JsonSerializerSettings()
        {
            // NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.Indented,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        [TestMethod]
        public void Given_ListMessagesResponse_Type_Then_It_Should_Contain_Properties()
        {
            var pis = typeof(ListMessagesResponse).GetProperties();

            pis.SingleOrDefault(p => p.Name == "Header").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(ResponseHeaderModel));
            pis.SingleOrDefault(p => p.Name == "Body").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(ListMessagesResponseBody));
        }

        [TestMethod]
        public void Given_ListMessagesResponseBody_Type_Then_It_Should_Contain_Properties()
        {
            var pis = typeof(ListMessagesResponseBody).GetProperties();

            pis.SingleOrDefault(p => p.Name == "PageNumber").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(int));
            pis.SingleOrDefault(p => p.Name == "PageSize").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(int));
            pis.SingleOrDefault(p => p.Name == "TotalCount").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(int));
            pis.SingleOrDefault(p => p.Name == "Data").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(List<ListMessagesResponseData>));

        }

        [TestMethod]
        public void Given_ListMessagesResponseData_Type_Then_It_Should_Contain_Properties()
        {
            var pis = typeof(ListMessagesResponseData).GetProperties();

            pis.SingleOrDefault(p => p.Name == "RequestId").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(string));
            pis.SingleOrDefault(p => p.Name == "RequestDate").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(string));
            pis.SingleOrDefault(p => p.Name == "ResponseDate").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(string));
            pis.SingleOrDefault(p => p.Name == "TemplateId").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(string));
            pis.SingleOrDefault(p => p.Name == "TemplateName").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(string));
            pis.SingleOrDefault(p => p.Name == "CategoryId").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(string));
            pis.SingleOrDefault(p => p.Name == "CategoryName").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(string));
            pis.SingleOrDefault(p => p.Name == "Body").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(string));
            pis.SingleOrDefault(p => p.Name == "SenderNumber").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(string));
            pis.SingleOrDefault(p => p.Name == "CountryCode").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(string));
            pis.SingleOrDefault(p => p.Name == "RecipientNumber").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(string));
            pis.SingleOrDefault(p => p.Name == "MessageStatus").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(string));
            pis.SingleOrDefault(p => p.Name == "MessageStatusName").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(string));
            pis.SingleOrDefault(p => p.Name == "ResultCode").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(string));
            pis.SingleOrDefault(p => p.Name == "ResultCodeName").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(string));
            pis.SingleOrDefault(p => p.Name == "CarrierCode").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(int));
            pis.SingleOrDefault(p => p.Name == "CarrierName").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(string));
            pis.SingleOrDefault(p => p.Name == "RecipientSequence").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(int));
            pis.SingleOrDefault(p => p.Name == "SendType").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(string));
            pis.SingleOrDefault(p => p.Name == "MessageType").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(string));
            pis.SingleOrDefault(p => p.Name == "UserId").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(string));
            pis.SingleOrDefault(p => p.Name == "IsAdvertisement").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(string));
            pis.SingleOrDefault(p => p.Name == "ResultMessage").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(string));
            pis.SingleOrDefault(p => p.Name == "SenderGroupKey").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(string));
            pis.SingleOrDefault(p => p.Name == "recipientGroupingKey").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(string));


        }


    }
}