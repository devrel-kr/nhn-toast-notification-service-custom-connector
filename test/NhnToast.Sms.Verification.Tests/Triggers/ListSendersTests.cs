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
using Toast.Sms.Verification.Configurations;
using Toast.Sms.Verification.Models;
using Toast.Sms.Verification.Tests.Configurations;
using Toast.Tests.Common.Configurations;

using WorldDomination.Net.Http;

namespace Toast.Sms.Verification.Tests.Triggers
{
    [TestClass]
    public class ListSendersTests
    {
        private RequestHeaderModel _headers;
        private ToastTestSettings<SmsVerificationEndpointSettings, SmsVerificationExamplesSettings> _settings;

        [TestInitialize]
        public void TestInit()
        {
            if (!this._headers.IsNullOrDefault() && !this._settings.IsNullOrDefault())
            {
                return;
            }

            var config = new ConfigurationBuilder().AddJsonFile("test.settings.json").Build();
            var headers = config.Get<RequestHeaderModel>(ToastSettings.Name);
            var settings = config.Get<ToastTestSettings<SmsVerificationEndpointSettings, SmsVerificationExamplesSettings>>(ToastSettings.Name);

            this._headers = headers;
            this._settings = settings;
        }

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
            var options = new ListSendersRequestUrlOptions()
            {
                Version = this._settings.Version,
                AppKey = this._headers.AppKey,
                SendNo = sendNo,
                UseYn = useYn,
                BlockYn = blockYn,
                PageNum = pageNum,
                PageSize = pageSize
            };
            var requestUrl = this._settings.Formatter.Format($"{this._settings.BaseUrl.TrimEnd('/')}/{this._settings.Endpoints.ListSenders.TrimStart('/')}", options);

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