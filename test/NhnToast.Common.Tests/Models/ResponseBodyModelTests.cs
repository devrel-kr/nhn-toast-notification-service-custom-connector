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
    public class ResponseBodyModelTests
    {
        private static JsonSerializerSettings serializerSettings = new JsonSerializerSettings()
        {
            // NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.Indented,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        [TestMethod]
        public void Given_ResponseItemBodyModel_Type_Then_It_Should_Contain_Properties()
        {
            var pis = typeof(FakeResponseItemBodyModel).GetProperties();

            pis.SingleOrDefault(p => p.Name == "Data").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(string));
        }

        [TestMethod]
        public void Given_ResponseCollectionBodyModel_Type_Then_It_Should_Contain_Properties()
        {
            var pis = typeof(FakeResponseCollectionBodyModel).GetProperties();

            pis.SingleOrDefault(p => p.Name == "PageNumber").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(int?));
            pis.SingleOrDefault(p => p.Name == "PageSize").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(int?));
            pis.SingleOrDefault(p => p.Name == "TotalCount").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(int?));
            pis.SingleOrDefault(p => p.Name == "Data").Should().NotBeNull()
               .And.Subject.PropertyType.Should().Be(typeof(List<string>));
        }



        [TestMethod]
        public void Given_Default_ResponseItemBodyModel_Instance_When_Serialised_Then_It_Should_Return_Result()
        {
            var model = new FakeResponseItemBodyModel();

            var serialised = JsonConvert.SerializeObject(model, serializerSettings);
            var deserialised = JsonConvert.DeserializeObject<Dictionary<string, object>>(serialised);

            deserialised.Keys.Should().Contain("data");

            var data = deserialised["data"];
            data.Should().BeNull();
        }

        [TestMethod]
        public void Given_Default_ResponseCollectionBodyModel_Instance_When_Serialised_Then_It_Should_Return_Result()
        {
            var model = new FakeResponseCollectionBodyModel();

            var serialised = JsonConvert.SerializeObject(model, serializerSettings);
            var deserialised = JsonConvert.DeserializeObject<Dictionary<string, object>>(serialised);

            deserialised.Keys.Should().Contain("pageNum");
            deserialised.Keys.Should().Contain("pageSize");
            deserialised.Keys.Should().Contain("totalCount");
            deserialised.Keys.Should().Contain("data");

            var pageNum = deserialised["pageNum"];
            pageNum.Should().BeNull();
            var pageSize = deserialised["pageSize"];
            pageSize.Should().BeNull();
            var totalCount = deserialised["totalCount"];
            totalCount.Should().BeNull();
            var data = deserialised["data"];
            data.Should().BeNull();
        }

        [DataTestMethod]
        [DataRow("hello world")]
        public void Given_Values_ResponseItemBodyModel_When_Serialised_Then_It_Should_Return_Result(string data)
        {
            var model = new FakeResponseItemBodyModel()
            {
                Data = data
            };

            var serialised = JsonConvert.SerializeObject(model, serializerSettings);
            var deserialised = JsonConvert.DeserializeObject<Dictionary<string, object>>(serialised);

            deserialised.Keys.Should().Contain("data");

            var dataProperty = deserialised["data"];
            dataProperty.Should().Be(data);
        }

        [DataTestMethod]
        [DataRow(1, 15, 15, "Hello", "World!")]
        public void Given_Values_ResponseCollectionBodyModel_When_Serialised_Then_It_Should_Return_Result(int pageNum, int pageSize, int totalCount, params string[] data)
        {
            var model = new FakeResponseCollectionBodyModel()
            {
                PageNumber = pageNum,
                PageSize = pageSize,
                TotalCount = totalCount,
                Data = data.ToList()
            };

            var serialised = JsonConvert.SerializeObject(model, serializerSettings);
            var deserialised = JsonConvert.DeserializeObject<Dictionary<string, object>>(serialised);

            deserialised.Keys.Should().Contain("pageNum");
            deserialised.Keys.Should().Contain("pageSize");
            deserialised.Keys.Should().Contain("totalCount");
            deserialised.Keys.Should().Contain("data");

            var pageNumProperty = deserialised["pageNum"];
            pageNumProperty.Should().Be(pageNum);
            var pageSizeProperty = deserialised["pageSize"];
            pageSizeProperty.Should().Be(pageSize);
            var totalCountProperty = deserialised["totalCount"];
            totalCountProperty.Should().Be(totalCount);
            var dataProperty = ((JArray)deserialised["data"]).ToObject<List<string>>();
            dataProperty.Should().Equal(data);
        }
    }
}