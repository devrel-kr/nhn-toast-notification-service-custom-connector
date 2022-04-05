using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configurations.AppSettings.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartFormat;
using WorldDomination.Net.Http;

namespace Toast.Sms.Verification.Tests.Triggers
{
    [TestClass]
    public class ListSendersTests
    {
        [TestMethod]
        public async Task Given_Parameters_With_Null_When_ListSenders_Invoked_Then_It_Should_Return_False()
        {
            // Arrange
            var config = new ConfigurationBuilder().AddJsonFile("test.settings.json").Build();
            var appKey = config.GetValue<string>("Toast:AppKey");
            var secretKey = config.GetValue<string>("Toast:SecretKey");
            var baseUrl = config.GetValue<string>("Toast:BaseUrl");
            var version = config.GetValue<string>("Toast:Version");
            var endpoint = config.GetValue<string>("Toast:Endpoints:ListSenders");
            var options = new
            {
                version = version,
                appKey = appKey,
                sendNo = default(string),
                useYn = default(string),
                blockYn = default(string),
                pageNum = default(string),
                pageSize = default(string)
            };
            var requestUrl = Smart.Format($"{baseUrl.TrimEnd('/')}/{endpoint.TrimStart('/')}", options);

            var http = new HttpClient();

            // Act
            http.DefaultRequestHeaders.Add("X-Secret-Key", secretKey);
            var result = await http.GetAsync(requestUrl).ConfigureAwait(false);

            dynamic payload = await result.Content.ReadAsAsync<object>().ConfigureAwait(false);

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            ((bool)payload.header.isSuccessful).Should().BeFalse();

            result.Dispose();
        }

        [TestMethod]
        public async Task Given_Parameters_With_Default_When_ListSenders_Invoked_Then_It_Should_Return_True()
        {
            // Arrange
            var config = new ConfigurationBuilder().AddJsonFile("test.settings.json").Build();
            var appKey = config.GetValue<string>("Toast:AppKey");
            var secretKey = config.GetValue<string>("Toast:SecretKey");
            var baseUrl = config.GetValue<string>("Toast:BaseUrl");
            var version = config.GetValue<string>("Toast:Version");
            var endpoint = config.GetValue<string>("Toast:Endpoints:ListSenders");
            var options = new
            {
                version = version,
                appKey = appKey,
                sendNo = default(string),
                useYn = default(string),
                blockYn = default(string),
                pageNum = 1,
                pageSize = 15
            };
            var requestUrl = Smart.Format($"{baseUrl.TrimEnd('/')}/{endpoint.TrimStart('/')}", options);

            var http = new HttpClient();

            // Act
            http.DefaultRequestHeaders.Add("X-Secret-Key", secretKey);
            var result = await http.GetAsync(requestUrl).ConfigureAwait(false);

            dynamic payload = await result.Content.ReadAsAsync<object>().ConfigureAwait(false);

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            ((bool)payload.header.isSuccessful).Should().BeTrue();

            result.Dispose();
        }
    }
}