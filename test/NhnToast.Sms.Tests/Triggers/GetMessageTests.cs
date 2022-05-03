using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configurations.AppSettings.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Toast.Common.Configurations;
using Toast.Common.Models;
using Toast.Sms.Configurations;
using Toast.Sms.Models;
using Toast.Sms.Tests.Configurations;
using Toast.Tests.Common.Configurations;

using WorldDomination.Net.Http;

namespace Toast.Sms.Tests.Triggers
{
    [TestClass]
    public class GetMessageTests
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
        [DataRow(false, null, false)]
        [DataRow(false, 1, false)]
        [DataRow(true, null, false)]
        [DataRow(true, 1, true)]
        [DataRow(true, 100, true)]
        public async Task Given_Parameters_When_GetMessage_Invoked_Then_It_Should_Return_Result(bool useRequestId, int? recipientSeq, bool expected)
        {
            // Arrange
            var options = new GetMessageRequestUrlOptions()
            {
                Version = this._settings.Version,
                AppKey = this._headers.AppKey,
                RequestId = useRequestId ? this._settings.Examples.RequestId : null,
                RecipientSeq = recipientSeq
            };
            var requestUrl = this._settings.Formatter.Format($"{this._settings.BaseUrl.TrimEnd('/')}/{this._settings.Endpoints.GetMessage.TrimStart('/')}", options);

            var http = new HttpClient();

            // Act
            http.DefaultRequestHeaders.Add("X-Secret-Key", this._headers.SecretKey);
            var result = await http.GetAsync(requestUrl).ConfigureAwait(false);

            dynamic payload = await result.Content.ReadAsAsync<object>().ConfigureAwait(false);

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            ((bool)payload.header.isSuccessful).Should().Be(expected);

            result.Dispose();
        }
    }
}