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
        [TestCategory("Integration")]
        [DataTestMethod]
        [DataRow(null, null, null, null, null, false)]
        [DataRow(null, null, null, null, 15, false)]
        [DataRow(null, null, null, 1, null, false)]
        [DataRow(null, null, null, 1, 15, true)]
        [DataRow("0101234567", null, null, 1, 15, true)]
        [DataRow(null, "Y", null, 1, 15, true)]
        [DataRow(null, null, "Y", 1, 15, true)]
        [DataRow("0101234567", "Y", null, 1, 15, true)]
        [DataRow("0101234567", null, "Y", 1, 15, true)]
        [DataRow(null, "Y", "Y", 1, 15, true)]
        [DataRow("0101234567", "Y", "N", 1, 15, true)]
        public async Task Given_Parameters_When_ListSenders_Invoked_Then_It_Should_Return_Result(string sendNo, string useYn, string blockYn, int? pageNum, int? pageSize, bool expected)
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
                sendNo = sendNo,
                useYn = useYn,
                blockYn = blockYn,
                pageNum = pageNum,
                pageSize = pageSize
            };
            var requestUrl = Smart.Format($"{baseUrl.TrimEnd('/')}/{endpoint.TrimStart('/')}", options);

            var http = new HttpClient();

            // Act
            http.DefaultRequestHeaders.Add("X-Secret-Key", secretKey);
            var result = await http.GetAsync(requestUrl).ConfigureAwait(false);

            dynamic payload = await result.Content.ReadAsAsync<object>().ConfigureAwait(false);

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            ((bool)payload.header.isSuccessful).Should().Be(expected);

            result.Dispose();
        }
    }
}