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
    public class SendMessagesResponseModelTests
    {
        private static JsonSerializerSettings serializerSettings = new JsonSerializerSettings()
        {
            // NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.Indented,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        [TestMethod]
        public void Given_SendMessagesResponseData_Type_Then_It_Should_Contain_Properties()
        {
            var pis = typeof(SendMessagesResponseData).GetProperties();

            pis.SingleOrDefault(p => p.Name == "RequestId").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(string));
            pis.SingleOrDefault(p => p.Name == "StatusCode").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(string));
            pis.SingleOrDefault(p => p.Name == "SenderGroupKey").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(string));
            pis.SingleOrDefault(p => p.Name == "SendResults").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(List<SendMessagesResponseResult>));

        }

        [TestMethod]
        public void Given_SendMessagesResponseResult_Type_Then_It_Should_Contain_Properties()
        {
            var pis = typeof(SendMessagesResponseResult).GetProperties();

            pis.SingleOrDefault(p => p.Name == "RecipientNumber").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(string));
            pis.SingleOrDefault(p => p.Name == "ResultCode").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(int));
            pis.SingleOrDefault(p => p.Name == "ResultMessage").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(string));
            pis.SingleOrDefault(p => p.Name == "RecipientSequence").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(int));
            pis.SingleOrDefault(p => p.Name == "RecipientGroupKey").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(string));

        }


        [TestMethod]
        public void Given_Default_SendMessagesResponseData_Instance_When_Serialised_Then_It_Should_Return_Result()
        {
            var model = new SendMessagesResponseData();

            var serialised = JsonConvert.SerializeObject(model, serializerSettings);
            var deserialised = JsonConvert.DeserializeObject<Dictionary<string, object>>(serialised);

            deserialised.Keys.Should().Contain("requestId");
            deserialised.Keys.Should().Contain("statusCode");
            deserialised.Keys.Should().Contain("senderGroupingKey");
            deserialised.Keys.Should().Contain("sendResultList");

            var requestId = deserialised["requestId"];
            requestId.Should().BeNull();
            var statusCode = deserialised["statusCode"];
            statusCode.Should().BeNull();
            var senderGroupingKey = deserialised["senderGroupingKey"];
            senderGroupingKey.Should().BeNull();

            var sendResultList = deserialised["sendResultList"];
            sendResultList.Should().BeNull();


        }

        [TestMethod]
        public void Given_Default_SendMessagesResponseResult_Instance_When_Serialised_Then_It_Should_Return_Result()
        {
            var model = new SendMessagesResponseResult();

            var serialised = JsonConvert.SerializeObject(model, serializerSettings);
            var deserialised = JsonConvert.DeserializeObject<Dictionary<string, object>>(serialised);

            deserialised.Keys.Should().Contain("recipientNo");
            deserialised.Keys.Should().Contain("resultCode");
            deserialised.Keys.Should().Contain("resultMessage");
            deserialised.Keys.Should().Contain("recipientSeq");
            deserialised.Keys.Should().Contain("recipientGroupingKey");

            var recipientNo = deserialised["recipientNo"];
            recipientNo.Should().BeNull();
            var resultCode = deserialised["resultCode"];
            resultCode.Should().Be(0);
            var ResultMessage = deserialised["resultMessage"];
            ResultMessage.Should().BeNull();
            var recipientSeq = deserialised["recipientSeq"];
            recipientSeq.Should().Be(0);
            var recipientGroupingKey = deserialised["recipientGroupingKey"];
            recipientGroupingKey.Should().BeNull();

        }

        [DataTestMethod]
        [DataRow("id", "code", "senderkey", "010-0000-0000",1,"success",1,"recipientkey")]
        public void Given_Values_SendMessagesResponseData_When_Serialised_Then_It_Should_Return_Result(string requestId, string statusCode, string senderGroupingKey, string recipientNo, int resultCode, string resultMessage, int recipientSeq, string recipientGroupingKey)
        {
            var model = new SendMessagesResponseData()
            {
                RequestId = requestId,
                StatusCode = statusCode,
                SenderGroupKey = senderGroupingKey,
                SendResults = new List<SendMessagesResponseResult>() { 
                    new SendMessagesResponseResult(){
                        RecipientNumber = recipientNo,
                        ResultCode = resultCode,
                        ResultMessage = resultMessage,
                        RecipientSequence = recipientSeq,
                        RecipientGroupKey = recipientGroupingKey
                    }
                }
            };

            var serialised = JsonConvert.SerializeObject(model, serializerSettings);
            var deserialised = JsonConvert.DeserializeObject<Dictionary<string, object>>(serialised);

            deserialised.Keys.Should().Contain("requestId");
            deserialised.Keys.Should().Contain("statusCode");
            deserialised.Keys.Should().Contain("senderGroupingKey");
            deserialised.Keys.Should().Contain("sendResultList");

            var requestIdProperty = deserialised["requestId"];
            requestIdProperty.Should().Be(requestId);
            var statusCodeProperty = deserialised["statusCode"];
            statusCodeProperty.Should().Be(statusCode);
            var senderGroupingKeyProperty = deserialised["senderGroupingKey"];
            senderGroupingKeyProperty.Should().Be(senderGroupingKey);

            var sendResultListProperty = ((JArray)deserialised["sendResultList"]).ToObject<List<Dictionary<string, object>>>();
            sendResultListProperty[0].Keys.Should().Contain("recipientNo");
            sendResultListProperty[0].Keys.Should().Contain("resultCode");
            sendResultListProperty[0].Keys.Should().Contain("resultMessage");
            sendResultListProperty[0].Keys.Should().Contain("recipientSeq");
            sendResultListProperty[0].Keys.Should().Contain("recipientGroupingKey");


            sendResultListProperty[0]["recipientNo"].Should().Be(recipientNo);
            sendResultListProperty[0]["resultCode"].Should().Be(resultCode);
            sendResultListProperty[0]["resultMessage"].Should().Be(resultMessage);
            sendResultListProperty[0]["recipientSeq"].Should().Be(recipientSeq);
            sendResultListProperty[0]["recipientGroupingKey"].Should().Be(recipientGroupingKey);

        }
    }
}