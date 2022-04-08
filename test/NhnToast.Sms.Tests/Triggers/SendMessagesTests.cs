using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using System.Net.Http.Headers;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configurations.AppSettings.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartFormat;
using WorldDomination.Net.Http;

namespace Toast.Sms.Tests.Triggers
{
    [TestClass]
    public class SendMessagesTests
    {
        [DataTestMethod]
        [DataRow(true, true, true)]
        [DataRow(false, false, false)]
        [DataRow(true, false, true)]
        [DataRow(false, true, false)]
        public async Task Given_Parameters_When_SendMessages_Invoked_Then_It_Should_Return_Result(bool senderExist, bool recipientExist, bool expected)
        {
            // Arrange
            var config = new ConfigurationBuilder().AddJsonFile("test.settings.json").Build();
            var sender = senderExist ? config.GetValue<string>("Toast:Sender") : null;
            var recipient = recipientExist ? config.GetValue<string>("Toast:Recipient") : null;
            var json = $"{{\"body\":\"hello world\",\"sendNo\":\"{sender}\",\"recipientList\":[{{\"recipientNo\":\"{recipient}\"}}]}}";
            var appKey = config.GetValue<string>("Toast:AppKey");
            var secretKey = config.GetValue<string>("Toast:SecretKey");
            var baseUrl = config.GetValue<string>("Toast:BaseUrl");
            var version = config.GetValue<string>("Toast:Version");
            var endpoint = config.GetValue<string>("Toast:Endpoints:SendMessages");
            var options = new
            {
                version = version,
                appKey = appKey
            };
            var requestUrl = Smart.Format($"{baseUrl.TrimEnd('/')}/{endpoint.TrimStart('/')}", options);

            var http = new HttpClient();

            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            http.DefaultRequestHeaders.Add("X-Secret-Key", secretKey);

            var result = await http.PostAsync(requestUrl, content).ConfigureAwait(false);
            dynamic payload = await result.Content.ReadAsAsync<object>().ConfigureAwait(false);

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            ((bool)payload.header.isSuccessful).Should().Be(expected);

            result.Dispose();
        }
    }
}