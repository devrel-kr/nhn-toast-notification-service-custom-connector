using System.Collections.Generic;
using System.Linq;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using Toast.Sms.Models;


namespace Toast.Sms.Tests.Models
{
    [TestClass]
    public class GetMessageResponseModelTest
    {
        private static JsonSerializerSettings serializerSettings = new JsonSerializerSettings()
        {
            //NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        [TestMethod]
        public void Given_GetMessageResponseData_Type_Then_It_Should_Contain_Properties()
        {
            var pis = typeof(GetMessageResponseData).GetProperties();

            pis.SingleOrDefault(p => p.Name == "RequestId").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(string));
            pis.SingleOrDefault(p => p.Name == "RequestDate").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(string));
            pis.SingleOrDefault(p => p.Name == "ResultDate").Should().NotBeNull()
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
            pis.SingleOrDefault(p => p.Name == "RecipientGroupKey").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(string));
        }

        [TestMethod]
        public void Given_Default_Instance_When_Serialised_Then_It_Should_Return_Result()
        {
            var model = new GetMessageResponseData();

            var serialised = JsonConvert.SerializeObject(model, serializerSettings);
            var deserialised = JsonConvert.DeserializeObject<Dictionary<string, object>>(serialised);

            deserialised.Keys.Should().Contain("requestId");
            deserialised.Keys.Should().Contain("requestDate");
            deserialised.Keys.Should().Contain("resultDate");
            deserialised.Keys.Should().Contain("templateId");
            deserialised.Keys.Should().Contain("templateName");
            deserialised.Keys.Should().Contain("categoryId");
            deserialised.Keys.Should().Contain("categoryName");
            deserialised.Keys.Should().Contain("body");
            deserialised.Keys.Should().Contain("sendNo");
            deserialised.Keys.Should().Contain("countryCode");
            deserialised.Keys.Should().Contain("recipientNo");
            deserialised.Keys.Should().Contain("msgStatus");
            deserialised.Keys.Should().Contain("msgStatusName");
            deserialised.Keys.Should().Contain("resultCode");
            deserialised.Keys.Should().Contain("resultCodeName");
            deserialised.Keys.Should().Contain("telecomCode");
            deserialised.Keys.Should().Contain("telecomCodeName");
            deserialised.Keys.Should().Contain("recipientSeq");
            deserialised.Keys.Should().Contain("sendType");
            deserialised.Keys.Should().Contain("messageType");
            deserialised.Keys.Should().Contain("userId");
            deserialised.Keys.Should().Contain("adYn");
            deserialised.Keys.Should().Contain("resultMessage");
            deserialised.Keys.Should().Contain("senderGroupingKey");
            deserialised.Keys.Should().Contain("recipientGroupingKey");

            deserialised["requestId"].Should().BeNull();
            deserialised["requestDate"].Should().BeNull();
            deserialised["resultDate"].Should().BeNull();
            deserialised["templateId"].Should().BeNull();
            deserialised["templateName"].Should().BeNull();
            deserialised["categoryId"].Should().BeNull();
            deserialised["categoryName"].Should().BeNull();
            deserialised["body"].Should().BeNull();
            deserialised["sendNo"].Should().BeNull();
            deserialised["countryCode"].Should().BeNull();
            deserialised["recipientNo"].Should().BeNull();
            deserialised["msgStatus"].Should().BeNull();
            deserialised["msgStatusName"].Should().BeNull();
            deserialised["resultCode"].Should().BeNull();
            deserialised["resultCodeName"].Should().BeNull();
            deserialised["telecomCode"].Should().Be(0);
            deserialised["telecomCodeName"].Should().BeNull();
            deserialised["recipientSeq"].Should().Be(0);
            deserialised["sendType"].Should().BeNull();
            deserialised["messageType"].Should().BeNull();
            deserialised["userId"].Should().BeNull();
            deserialised["adYn"].Should().BeNull();
            deserialised["resultMessage"].Should().BeNull();
            deserialised["senderGroupingKey"].Should().BeNull();
            deserialised["recipientGroupingKey"].Should().BeNull();
        }

        [DataTestMethod]
        [DataRow("20180810100630ReZQ6KZzAH0", "2018-08-10 10:06:30.0", "2018-08-10 10:06:42.0", "TemplateId", "TemplateName", "0", "CategoryName", "body", "15446859", "82", "01000000000", "3", "success", "1000", " success", 10001, "SKT", 1, "0", "SMS", "tester", "N", "lorem ipsum", "senderGroupingKey", "recipientGroupingKey")]
        public void Given_Values_When_Serialised_Then_It_Should_Return_Result(string requestId, string requestDate, string resultDate, string templateId, string templateName, string categoryId, string categoryName, string body, string sendNo, string countryCode, string recipientNo, string msgStatus, string msgStatusName, string resultCode, string resultCodeName, int telecomCode, string telecomCodeName, int recipientSeq, string sendType, string messageType, string userId, string adYn, string resultMessage, string senderGroupingKey, string recipientGroupingKey)
        {
            var model = new GetMessageResponseData()
            {
                RequestId = requestId,
                RequestDate = requestDate,
                ResultDate = resultDate,
                TemplateId = templateId,
                TemplateName = templateName,
                CategoryId = categoryId,
                CategoryName = categoryName,
                Body = body,
                SenderNumber = sendNo,
                CountryCode = countryCode,
                RecipientNumber = recipientNo,
                MessageStatus = msgStatus,
                MessageStatusName = msgStatusName,
                ResultCode = resultCode,
                ResultCodeName = resultCodeName,
                CarrierCode = telecomCode,
                CarrierName = telecomCodeName,
                RecipientSequence = recipientSeq,
                SendType = sendType,
                MessageType = messageType,
                UserId = userId,
                IsAdvertisement = adYn,
                ResultMessage = resultMessage,
                SenderGroupKey = senderGroupingKey,
                RecipientGroupKey = recipientGroupingKey
            };

            var serialised = JsonConvert.SerializeObject(model, serializerSettings);
            var deserialised = JsonConvert.DeserializeObject<Dictionary<string, object>>(serialised);

            deserialised.Keys.Should().Contain("requestId");
            deserialised.Keys.Should().Contain("requestDate");
            deserialised.Keys.Should().Contain("resultDate");
            deserialised.Keys.Should().Contain("templateId");
            deserialised.Keys.Should().Contain("templateName");
            deserialised.Keys.Should().Contain("categoryId");
            deserialised.Keys.Should().Contain("categoryName");
            deserialised.Keys.Should().Contain("body");
            deserialised.Keys.Should().Contain("sendNo");
            deserialised.Keys.Should().Contain("countryCode");
            deserialised.Keys.Should().Contain("recipientNo"); 
            deserialised.Keys.Should().Contain("msgStatus");
            deserialised.Keys.Should().Contain("msgStatusName");
            deserialised.Keys.Should().Contain("resultCode");
            deserialised.Keys.Should().Contain("resultCodeName");
            deserialised.Keys.Should().Contain("telecomCode");
            deserialised.Keys.Should().Contain("telecomCodeName");
            deserialised.Keys.Should().Contain("recipientSeq");
            deserialised.Keys.Should().Contain("sendType");
            deserialised.Keys.Should().Contain("messageType");
            deserialised.Keys.Should().Contain("userId");
            deserialised.Keys.Should().Contain("adYn");
            deserialised.Keys.Should().Contain("resultMessage");
            deserialised.Keys.Should().Contain("senderGroupingKey");
            deserialised.Keys.Should().Contain("recipientGroupingKey");

            deserialised["requestId"].Should().Be(requestId);
            deserialised["requestDate"].Should().Be(requestDate);
            deserialised["resultDate"].Should().Be(resultDate);
            deserialised["requestDate"].Should().Be(requestDate);
            deserialised["templateName"].Should().Be(templateName);
            deserialised["categoryId"].Should().Be(categoryId);
            deserialised["categoryName"].Should().Be(categoryName);
            deserialised["body"].Should().Be(body);
            deserialised["sendNo"].Should().Be(sendNo);
            deserialised["countryCode"].Should().Be(countryCode);
            deserialised["recipientNo"].Should().Be(recipientNo);
            deserialised["msgStatus"].Should().Be(msgStatus);
            deserialised["msgStatusName"].Should().Be(msgStatusName);
            deserialised["resultCode"].Should().Be(resultCode);
            deserialised["resultCodeName"].Should().Be(resultCodeName);
            deserialised["telecomCode"].Should().Be(telecomCode);
            deserialised["telecomCodeName"].Should().Be(telecomCodeName);
            deserialised["recipientSeq"].Should().Be(recipientSeq);
            deserialised["sendType"].Should().Be(sendType);
            deserialised["messageType"].Should().Be(messageType);
            deserialised["userId"].Should().Be(userId);
            deserialised["adYn"].Should().Be(adYn);
            deserialised["resultMessage"].Should().Be(resultMessage);
            deserialised["senderGroupingKey"].Should().Be(senderGroupingKey);
            deserialised["recipientGroupingKey"].Should().Be(recipientGroupingKey);
        }
    }
}