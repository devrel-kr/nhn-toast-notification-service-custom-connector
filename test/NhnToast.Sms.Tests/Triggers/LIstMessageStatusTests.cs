using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configurations.AppSettings.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SmartFormat;

using WorldDomination.Net.Http;

using Toast.Sms.Models;

namespace Toast.Sms.Tests.Triggers
{
    [TestClass]
    public class ListMessageStatusTests
    {
        [TestCategory("Integration")]
        [DataTestMethod]
        [DataRow("2018-10-04 16:16:00", "2018-10-04 16:17:10", "SMS", 1, 15, true)]
        [DataRow("2018-10-04 16:16:00", "2018-10-04 16:17:10", "LMS", 1, 15, true)]
        [DataRow("2018-10-04 16:16:00", "2018-10-04 16:17:10", "MMS", 1, 15, true)]
        [DataRow("2018-10-04 16:16:00", "2018-10-04 16:17:10", "AUTH", 1, 15, true)]
        [DataRow(null, "2018-10-04 16:17:10", "MMS", 1, 15, false)]
        [DataRow("2018-10-04 16:16:00", null, "MMS", 1, 15, false)]
        [DataRow(null, null, "MMS", 1, 15, false)]
        [DataRow("2018-10-04 16:16:00", "2018-10-04 16:17:10", null, 1, 15, true)]
        [DataRow("2018-10-04 16:16:00", "2018-10-04 16:17:10", null, 0, 15, true)]
        [DataRow("2018-10-04 16:16:00", "2018-10-04 16:17:10", "SMS", null, 15, false)]
        [DataRow("2018-10-04 16:16:00", "2018-10-04 16:17:10", "SMS", 1, null, false)]
        public async Task Given_Parameters_When_ListMessagesStatus_Invoked_Then_It_Should_Return_Result(string startUpdateDate, string endUpdatedate, string? messageType, int? pageNum, int? pageSize, bool expected)
        {
            // Arrange
            var config = new ConfigurationBuilder().AddJsonFile("test.settings.json").Build();
            var appKey = config.GetValue<string>("Toast:AppKey");
            var secretKey = config.GetValue<string>("Toast:SecretKey");
            var baseUrl = config.GetValue<string>("Toast:BaseUrl");
            var version = config.GetValue<string>("Toast:Version");
            var endpoint = config.GetValue<string>("Toast:Endpoints:ListMessageStatus");
            var options = new ListMessageStatusRequestUrlOptions()
            {
                Version = version,
                AppKey = appKey,
                StartUpdateDate = startUpdateDate,
                EndUpdateDate = endUpdatedate,
                MessageType = messageType,
                PageNum = pageNum,
                PageSize = pageSize
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