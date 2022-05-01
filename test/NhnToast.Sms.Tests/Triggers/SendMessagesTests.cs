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
        [TestCategory("Integration")]
        [DataTestMethod]
        [DataRow(null, false, false, false)]
        [DataRow(null, false, true, false)]
        [DataRow(null, true, false, false)]
        [DataRow(null, true, true, false)]
        [DataRow("hello world", false, false, false)]
        [DataRow("hello world", false, true, false)]
        [DataRow("hello world", true, false, true)]
        [DataRow("hello world", true, true, true)]
        public async Task Given_Parameters_When_SendMessages_Invoked_Then_It_Should_Return_Result(string body, bool useSendNo, bool useRecipientNo, bool expected)
        {
            // Arrange
            var config = new ConfigurationBuilder().AddJsonFile("test.settings.json").Build();
            var sendNo = useSendNo ? config.GetValue<string>("Toast:Examples:SendNo") : null;
            var recipientNo = useRecipientNo ? config.GetValue<string>("Toast:Examples:RecipientNo") : null;
            var json = $"{{ \"body\": \"{body}\", \"sendNo\": \"{sendNo}\", \"recipientList\": [ {{\"recipientNo\": \"{recipientNo}\" }} ] }}";
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