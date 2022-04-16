using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configurations.AppSettings.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SmartFormat;

using WorldDomination.Net.Http;

namespace Toast.Sms.Tests.Triggers
{
    [TestClass]
    public class GetMessageTests
    {
        [DataTestMethod]
        [DataRow(false, null, false)]
        [DataRow(false, 1, false)]
        [DataRow(true, null, false)]
        [DataRow(true, 1, true)]
        [DataRow(true, 100, true)]
        public async Task Given_Parameters_When_GetMessage_Invoked_Then_It_Should_Return_Result(bool useRequestId, int? recipientSeq, bool expected)
        {
            // Arrange
            var config = new ConfigurationBuilder().AddJsonFile("test.settings.json").Build();
            var appKey = config.GetValue<string>("Toast:AppKey");
            var secretKey = config.GetValue<string>("Toast:SecretKey");
            var baseUrl = config.GetValue<string>("Toast:BaseUrl");
            var version = config.GetValue<string>("Toast:Version");
            var endpoint = config.GetValue<string>("Toast:Endpoints:GetMessage");
            var requestId = useRequestId ? config.GetValue<string>("Toast:Examples:RequestId") : null;
            var options = new GetMessageRequestUrlOptions()
            {
                version = version,
                appKey = appKey,
                requestId = requestId,
                recipientSeq = recipientSeq
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