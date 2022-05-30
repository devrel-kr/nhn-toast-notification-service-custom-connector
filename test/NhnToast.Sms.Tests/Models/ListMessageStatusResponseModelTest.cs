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
    public class ListMessageStatusResponseModelTest
    {
        private static JsonSerializerSettings serializerSettings = new JsonSerializerSettings()
        {
            // NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        [TestMethod]
        public void Given_ListMessageStatusResponseData_Type_Then_It_Should_Contain_Properties()
        {
            var pis = typeof(ListMessageStatusResponseData).GetProperties();

            pis.SingleOrDefault(p => p.Name == "MessageType").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(string));
            pis.SingleOrDefault(p => p.Name == "RequestId").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(string));
            pis.SingleOrDefault(p => p.Name == "RecipientSequence").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(int));
            pis.SingleOrDefault(p => p.Name == "ResultCode").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(string));
            pis.SingleOrDefault(p => p.Name == "ResultName").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(string));
            pis.SingleOrDefault(p => p.Name == "RequestDate").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(string));
            pis.SingleOrDefault(p => p.Name == "ResultDate").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(string));
            pis.SingleOrDefault(p => p.Name == "UpdateDate").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(string));
            pis.SingleOrDefault(p => p.Name == "CarrierCode").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(string));
            pis.SingleOrDefault(p => p.Name == "CarrierName").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(string));
            pis.SingleOrDefault(p => p.Name == "SenderGroupKey").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(string));
            pis.SingleOrDefault(p => p.Name == "RecipientGroupKey").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(string));
        }

        [TestMethod]
        public void Given_Default_Instance_When_Serialised_Then_It_Should_Return_Result()
        {
            var model = new ListMessageStatusResponseData();

            var serialised = JsonConvert.SerializeObject(model, serializerSettings);
            var deserialised = JsonConvert.DeserializeObject<Dictionary<string, object>>(serialised);


            deserialised.Keys.Should().Contain("messageType");
            deserialised.Keys.Should().Contain("requestId");
            deserialised.Keys.Should().Contain("recipientSeq");
            deserialised.Keys.Should().Contain("resultCode");
            deserialised.Keys.Should().Contain("resultCodeName");
            deserialised.Keys.Should().Contain("requestDate");
            deserialised.Keys.Should().Contain("resultDate");
            deserialised.Keys.Should().Contain("updateDate");
            deserialised.Keys.Should().Contain("telecomCode");
            deserialised.Keys.Should().Contain("telecomCodeName");
            deserialised.Keys.Should().Contain("senderGroupingKey");
            deserialised.Keys.Should().Contain("recipientGroupingKey");

            deserialised["messageType"].Should().BeNull();
            deserialised["requestId"].Should().BeNull();
            deserialised["recipientSeq"].Should().Be(0);
            deserialised["resultCode"].Should().BeNull();
            deserialised["resultCodeName"].Should().BeNull();
            deserialised["requestDate"].Should().BeNull();
            deserialised["resultDate"].Should().BeNull();
            deserialised["updateDate"].Should().BeNull();
            deserialised["telecomCode"].Should().BeNull();
            deserialised["telecomCodeName"].Should().BeNull();
            deserialised["senderGroupingKey"].Should().BeNull();
            deserialised["recipientGroupingKey"].Should().BeNull();
        }

        [DataTestMethod]
        [DataRow("sms", "lorem ipsum", 0, "1000", "success", "2018-10-04 16:16:00.0", "2018 - 10 - 04 16:17:10.0", "2018-10-04 16:17:15.0", "10003", "LGU", "senderGroupingKey", "recipientGroupingKey")]
        public void Given_Values_When_Serialised_Then_It_Should_Return_Result(string messageType, string requestId, int recipientSeq, string resultCode, string resultCodeName, string requestDate, string resultDate, string updateDate, string telecomCode, string telecomCodeName, string senderGroupingKey, string recipientGroupingKey)
        {
            var model = new ListMessageStatusResponseData()
            {
                MessageType = messageType,
                RequestId = requestId,
                RecipientSequence = recipientSeq,
                ResultCode = resultCode,
                ResultName = resultCodeName,
                RequestDate = requestDate,
                ResultDate = resultDate,
                UpdateDate = updateDate,
                CarrierCode = telecomCode,
                CarrierName = telecomCodeName,
                SenderGroupKey = senderGroupingKey,
                RecipientGroupKey = recipientGroupingKey
            };

            var serialised = JsonConvert.SerializeObject(model, serializerSettings);
            var deserialised = JsonConvert.DeserializeObject<Dictionary<string, object>>(serialised);

            deserialised.Keys.Should().Contain("messageType");
            deserialised.Keys.Should().Contain("requestId");
            deserialised.Keys.Should().Contain("recipientSeq");
            deserialised.Keys.Should().Contain("resultCode");
            deserialised.Keys.Should().Contain("resultCodeName");
            deserialised.Keys.Should().Contain("requestDate");
            deserialised.Keys.Should().Contain("resultDate");
            deserialised.Keys.Should().Contain("updateDate");
            deserialised.Keys.Should().Contain("telecomCode");
            deserialised.Keys.Should().Contain("telecomCodeName");
            deserialised.Keys.Should().Contain("senderGroupingKey");
            deserialised.Keys.Should().Contain("recipientGroupingKey");


            deserialised["messageType"].Should().Be(messageType);
            deserialised["requestId"].Should().Be(requestId);
            deserialised["recipientSeq"].Should().Be(recipientSeq);
            deserialised["resultCode"].Should().Be(resultCode);
            deserialised["resultCodeName"].Should().Be(resultCodeName);
            deserialised["requestDate"].Should().Be(requestDate);
            deserialised["resultDate"].Should().Be(resultDate);
            deserialised["updateDate"].Should().Be(updateDate);
            deserialised["telecomCode"].Should().Be(telecomCode);
            deserialised["telecomCodeName"].Should().Be(telecomCodeName);
            deserialised["senderGroupingKey"].Should().Be(senderGroupingKey);
            deserialised["recipientGroupingKey"].Should().Be(recipientGroupingKey);
        }
    }
}
