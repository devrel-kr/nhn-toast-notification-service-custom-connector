using System.Collections.Generic;
using System.Linq;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

using Toast.Common.Models;
using Toast.Common.Tests.Fakes;

namespace Toast.Common.Tests.Models
{
    [TestClass]
    public class ResponseModelTests
    {
        private static JsonSerializerSettings serializerSettings = new JsonSerializerSettings()
        {
            // NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.Indented,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        [TestMethod]
        public void Given_ResponseModel_Type_Then_It_Should_Contain_Properties()
        {
            var pis = typeof(FakeResponseModel).GetProperties();

            pis.SingleOrDefault(p => p.Name == "Header").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(ResponseHeaderModel));
            pis.SingleOrDefault(p => p.Name == "Body").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(string));
        }

        [TestMethod]
        public void Given_ResponseHeaderModel_Type_Then_It_Should_Contain_Properties()
        {
            var pis = typeof(ResponseHeaderModel).GetProperties();

            pis.SingleOrDefault(p => p.Name == "IsSuccessful").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(bool));
            pis.SingleOrDefault(p => p.Name == "ResultCode").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(int));
            pis.SingleOrDefault(p => p.Name == "ResultMessage").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(string));
        }

        [TestMethod]
        public void Given_Default_Instance_When_Serialised_Then_It_Should_Return_Result()
        {
            var model = new FakeResponseModel();

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
            var model = new FakeResponseModel()
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