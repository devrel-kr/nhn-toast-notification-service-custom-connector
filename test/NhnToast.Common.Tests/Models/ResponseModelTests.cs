using System.Collections.Generic;
using System.Linq;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;
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

        [TestInitialize]
        public void Init()
        {

        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Contain_Header()
        {
            var pis = typeof(FakeResponseModel).GetProperties();

            var pi = pis.SingleOrDefault(p => p.Name == "Header");

            pi.Should().NotBeNull();
            pi.PropertyType.Should().Be(typeof(ResponseHeaderModel));

            var attr = pi.CustomAttributes.SingleOrDefault(a => a.AttributeType == typeof(JsonPropertyAttribute));

            attr.Should().NotBeNull();
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Contain_Body()
        {
            var pis = typeof(FakeResponseModel).GetProperties();

            var pi = pis.SingleOrDefault(p => p.Name == "Body");

            pi.Should().NotBeNull();
            pi.PropertyType.Should().Be(typeof(string));

            var attr = pi.CustomAttributes.SingleOrDefault(a => a.AttributeType == typeof(JsonPropertyAttribute));

            attr.Should().NotBeNull();
        }

        [TestMethod]
        public void Given_Default_Instance_When_Serialized_Then_It_Should_Return_Result()
        {
            var model = new FakeResponseModel();

            var serialised = JsonConvert.SerializeObject(model, serializerSettings);
            var deserialised = JsonConvert.DeserializeObject<Dictionary<string, object>>(serialised);

            deserialised.Keys.Should().Contain("header");
            deserialised.Keys.Should().Contain("body");
            
            // var header = deserialised["header"] as Dictionary<string, object>;
            // header.Keys.Should().Contain("isSuccessful");
            // header.Keys.Should().Contain("resultCode");
            // header.Keys.Should().Contain("resultMessage");

            // header["isSuccessful"].Should().Be(false);
            // header["resultCode"].Should().Be(0);
            // header["resultMessage"].Should().BeNull();

            deserialised["body"].Should().BeNull();
        }

        [DataTestMethod]
        [DataRow(true, 1, "lorem ipsum", "hello world")]
        public void Given_Values_When_Serialized_Then_It_Should_Return_Result(bool isSuccessful, int resultCode, string resultMessage, string body)
        {
            var model = new FakeResponseModel()
            {
                Header = new ResponseHeaderModel()
                {
                    IsSuccessful = isSuccessful,
                    ResultCode = resultCode,
                    ResultMessage = resultMessage
                },
                Body = body
            };

            var serialised = JsonConvert.SerializeObject(model, serializerSettings);
            var deserialised = JsonConvert.DeserializeObject<Dictionary<string, object>>(serialised);

            deserialised.Keys.Should().Contain("header");
            deserialised.Keys.Should().Contain("body");
            
            // var header = deserialised["header"] as Dictionary<string, object>;
            // header.Keys.Should().Contain("isSuccessful");
            // header.Keys.Should().Contain("resultCode");
            // header.Keys.Should().Contain("resultMessage");

            // header["isSuccessful"].Should().Be(false);
            // header["resultCode"].Should().Be(0);
            // header["resultMessage"].Should().BeNull();

            deserialised["body"].Should().Be(body);
        }
    }
}