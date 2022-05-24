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
            pis.SingleOrDefault(p => p.Name == "RecipientGroupKey").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(string));


        }

        [TestMethod]
        public void Given_Default_ListMessagesResponseData_Instance_When_Serialised_Then_It_Should_Return_Result()
        {
            var model = new ListMessagesResponseData();

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

            var requestId = deserialised["requestId"];
            requestId.Should().BeNull();
            var requestDate = deserialised["requestDate"];
            requestDate.Should().BeNull();
            var resultDate = deserialised["resultDate"];
            resultDate.Should().BeNull();
            var templateId = deserialised["templateId"];
            templateId.Should().BeNull();
            var templateName = deserialised["templateName"];
            templateName.Should().BeNull();
            var categoryId = deserialised["categoryId"];
            categoryId.Should().BeNull();
            var categoryName = deserialised["categoryName"];
            categoryName.Should().BeNull();
            var body = deserialised["body"];
            body.Should().BeNull();
            var sendNo = deserialised["sendNo"];
            sendNo.Should().BeNull();
            var countryCode = deserialised["countryCode"];
            countryCode.Should().BeNull();
            var recipientNo = deserialised["recipientNo"];
            recipientNo.Should().BeNull();
            var msgStatus = deserialised["msgStatus"];
            msgStatus.Should().BeNull();
            var msgStatusName = deserialised["msgStatusName"];
            msgStatusName.Should().BeNull();
            var resultCode = deserialised["resultCode"];
            resultCode.Should().BeNull();
            var resultCodeName = deserialised["resultCodeName"];
            resultCodeName.Should().BeNull();
            var telecomCode = deserialised["telecomCode"];
            telecomCode.Should().Be(0);
            var telecomCodeName = deserialised["telecomCodeName"];
            telecomCodeName.Should().BeNull();
            var recipientSeq = deserialised["recipientSeq"];
            recipientSeq.Should().Be(0);
            var sendType = deserialised["sendType"];
            sendType.Should().BeNull();
            var messageType = deserialised["messageType"];
            messageType.Should().BeNull();
            var userId = deserialised["userId"];
            userId.Should().BeNull();
            var adYn = deserialised["adYn"];
            adYn.Should().BeNull();
            var resultMessage = deserialised["resultMessage"];
            resultMessage.Should().BeNull();
            var senderGroupingKey = deserialised["senderGroupingKey"];
            senderGroupingKey.Should().BeNull();
            var recipientGroupingKey = deserialised["recipientGroupingKey"];
            recipientGroupingKey.Should().BeNull();


        }

        [DataTestMethod]
        [DataRow("id","2022-05-20","2022-05-20","tid","tname","cid","cname","hello","010-0000-0000","00","010-0000-0000","0","success","0","success",0,"L",1,"mms","mms","uid","y","rmsg","skey","rkey")]
        public void Given_Values_ListMessagesResponseData_When_Serialised_Then_It_Should_Return_Result(string requestId, string requestDate, string resultDate, string templateId, string templateName, string categoryId, string categoryName, string body, string sendNo, string countryCode, string recipientNo, string msgStatus, string msgStatusName, string resultCode, string resultCodeName, int telecomCode, string telecomCodeName, int recipientSeq, string sendType, string messageType, string userId, string adYn, string resultMessage, string senderGroupingKey, string recipientGroupingKey)
        {
            var model = new ListMessagesResponseData()
            {
                RequestId = requestId,
                RequestDate = requestDate,
                ResponseDate = resultDate,
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

            var requestIdProperty = deserialised["requestId"];
            requestIdProperty.Should().Be(requestId);
            var requestDateProperty = deserialised["requestDate"];
            requestDateProperty.Should().Be(requestDate);
            var resultDateProperty = deserialised["resultDate"];
            resultDateProperty.Should().Be(resultDate);
            var templateIdProperty = deserialised["templateId"];
            templateIdProperty.Should().Be(templateId);
            var templateNameProperty = deserialised["templateName"];
            templateNameProperty.Should().Be(templateName);
            var categoryIdProperty = deserialised["categoryId"];
            categoryIdProperty.Should().Be(categoryId);
            var categoryNameProperty = deserialised["categoryName"];
            categoryNameProperty.Should().Be(categoryName);
            var bodyProperty = deserialised["body"];
            bodyProperty.Should().Be(body);
            var sendNoProperty = deserialised["sendNo"];
            sendNoProperty.Should().Be(sendNo);
            var countryCodeProperty = deserialised["countryCode"];
            countryCodeProperty.Should().Be(countryCode);
            var recipientNoProperty = deserialised["recipientNo"];
            recipientNoProperty.Should().Be(recipientNo);
            var msgStatusProperty = deserialised["msgStatus"];
            msgStatusProperty.Should().Be(msgStatus);
            var msgStatusNameProperty = deserialised["msgStatusName"];
            msgStatusNameProperty.Should().Be(msgStatusName);
            var resultCodeProperty = deserialised["resultCode"];
            resultCodeProperty.Should().Be(resultCode);
            var resultCodeNameProperty = deserialised["resultCodeName"];
            resultCodeNameProperty.Should().Be(resultCodeName);
            var telecomCodeProperty = deserialised["telecomCode"];
            telecomCodeProperty.Should().Be(telecomCode);
            var telecomCodeNameProperty = deserialised["telecomCodeName"];
            telecomCodeNameProperty.Should().Be(telecomCodeName);
            var recipientSeqProperty = deserialised["recipientSeq"];
            recipientSeqProperty.Should().Be(recipientSeq);
            var sendTypeProperty = deserialised["sendType"];
            sendTypeProperty.Should().Be(sendType);
            var messageTypeProperty = deserialised["messageType"];
            messageTypeProperty.Should().Be(messageType);
            var userIdProperty = deserialised["userId"];
            userIdProperty.Should().Be(userId);
            var adYnProperty = deserialised["adYn"];
            adYnProperty.Should().Be(adYn);
            var resultMessageProperty = deserialised["resultMessage"];
            resultMessageProperty.Should().Be(resultMessage);
            var senderGroupingKeyProperty = deserialised["senderGroupingKey"];
            senderGroupingKeyProperty.Should().Be(senderGroupingKey);
            var recipientGroupingKeyProperty = deserialised["recipientGroupingKey"];
            recipientGroupingKeyProperty.Should().Be(recipientGroupingKey);


        }


    }
}