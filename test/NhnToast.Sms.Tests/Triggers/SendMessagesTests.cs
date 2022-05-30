using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configurations.AppSettings.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Toast.Common.Builders;
using Toast.Common.Configurations;
using Toast.Common.Models;
using Toast.Sms.Configurations;
using Toast.Sms.Tests.Configurations;
using Toast.Tests.Common.Configurations;

namespace Toast.Sms.Tests.Triggers
{
    [TestClass]
    public class SendMessagesTests
    {
        private RequestHeaderModel _headers;
        private ToastTestSettings<SmsEndpointSettings, SmsExamplesSettings> _settings;

        [TestInitialize]
        public void TestInit()
        {
            if (!this._headers.IsNullOrDefault() && !this._settings.IsNullOrDefault())
            {
                return;
            }

            var config = new ConfigurationBuilder().AddJsonFile("test.settings.json").Build();
            var headers = config.Get<RequestHeaderModel>(ToastSettings.Name);
            var settings = config.Get<ToastTestSettings<SmsEndpointSettings, SmsExamplesSettings>>(ToastSettings.Name);

            this._headers = headers;
            this._settings = settings;
        }

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
            var sendNo = useSendNo ? this._settings.Examples.SendNo : null;
            var recipientNo = useRecipientNo ? this._settings.Examples.RecipientNo : null;
            var json = $"{{ \"body\": \"{body}\", \"sendNo\": \"{sendNo}\", \"recipientList\": [ {{\"recipientNo\": \"{recipientNo}\" }} ] }}";
            
            var requestUrl = new RequestUrlBuilder()
                .WithSettings(this._settings, this._settings.Endpoints.SendMessages)
                .WithHeaders(this._headers)
                .Build();

            var http = new HttpClient();

            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            http.DefaultRequestHeaders.Add("X-Secret-Key", this._headers.SecretKey);

            var result = await http.PostAsync(requestUrl, content).ConfigureAwait(false);
            dynamic payload = await result.Content.ReadAsAsync<object>().ConfigureAwait(false);

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            ((bool)payload.header.isSuccessful).Should().Be(expected);

            result.Dispose();
        }
    }
}