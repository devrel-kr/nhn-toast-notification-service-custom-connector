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
        public void Given_SendMessagesResponse_Type_Then_It_Should_Contain_Properties()
        {
            var pis = typeof(SendMessagesResponse).GetProperties();

            pis.SingleOrDefault(p => p.Name == "Header").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(ResponseHeaderModel));
            pis.SingleOrDefault(p => p.Name == "Body").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(SendMessagesResponseBody));
        }

        [TestMethod]
        public void Given_SendMessagesResponseBody_Type_Then_It_Should_Contain_Properties()
        {
            var pis = typeof(SendMessagesResponseBody).GetProperties();

            pis.SingleOrDefault(p => p.Name == "Data").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(SendMessagesResponseData));

        }

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
        public void Given_Default_Instance_When_Serialised_Then_It_Should_Return_Result()
        {
            var model = new FakeSendMessagesResponseModel();

            var serialised = JsonConvert.SerializeObject(model, serializerSettings);
            var deserialised = JsonConvert.DeserializeObject<Dictionary<string, object>>(serialised);

            deserialised.Keys.Should().Contain("header");
            deserialised.Keys.Should().Contain("body");

            var header = ((JObject)deserialised["header"]).ToObject<Dictionary<string, object>>();
            header.Keys.Should().Contain("isSuccessful");
            header.Keys.Should().Contain("resultCode");
            header.Keys.Should().Contain("resultMessage");

            header["isSuccessful"].Should().Be(false);
            header["resultCode"].Should().Be(0);
            header["resultMessage"].Should().BeNull();

            var body = deserialised["body"];
            body.Should().BeNull();
        }

        [DataTestMethod]
        [DataRow(true, 1, "lorem ipsum", "hello world")]
        public void Given_Values_When_Serialised_Then_It_Should_Return_Result(bool isSuccessful, int resultCode, string resultMessage, string bodyMessage)
        {
            var model = new FakeSendMessagesResponseModel()
            {
                Header = new ResponseHeaderModel()
                {
                    IsSuccessful = isSuccessful,
                    ResultCode = resultCode,
                    ResultMessage = resultMessage
                },
                Body = bodyMessage
            };

            var serialised = JsonConvert.SerializeObject(model, serializerSettings);
            var deserialised = JsonConvert.DeserializeObject<Dictionary<string, object>>(serialised);

            deserialised.Keys.Should().Contain("header");
            deserialised.Keys.Should().Contain("body");

            var header = ((JObject)deserialised["header"]).ToObject<Dictionary<string, object>>();
            header.Keys.Should().Contain("isSuccessful");
            header.Keys.Should().Contain("resultCode");
            header.Keys.Should().Contain("resultMessage");

            header["isSuccessful"].Should().Be(isSuccessful);
            header["resultCode"].Should().Be(resultCode);
            header["resultMessage"].Should().Be(resultMessage);

            var body = deserialised["body"];
            body.Should().Be(bodyMessage);
        }
    }
}