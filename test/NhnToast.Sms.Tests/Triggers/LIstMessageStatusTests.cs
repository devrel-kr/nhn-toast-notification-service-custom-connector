using System.Net;
using System.Net.Http;
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
using Toast.Sms.Models;
using Toast.Sms.Tests.Configurations;
using Toast.Tests.Common.Configurations;

namespace Toast.Sms.Tests.Triggers
{
    [TestClass]
    public class ListMessageStatusTests
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
        [DataRow("2018-10-04 16:16:00", "2018-10-04 16:17:10", "SMS", 1, 15, true)]
        [DataRow("2018-10-04 16:16:00", "2018-10-04 16:17:10", "LMS", 1, 15, true)]
        [DataRow("2018-10-04 16:16:00", "2018-10-04 16:17:10", "MMS", 1, 15, true)]
        [DataRow("2018-10-04 16:16:00", "2018-10-04 16:17:10", "AUTH", 1, 15, true)]
        [DataRow(null, "2018-10-04 16:17:10", "MMS", 1, 15, false)]
        [DataRow("2018-10-04 16:16:00", null, "MMS", 1, 15, false)]
        [DataRow(null, null, "MMS", 1, 15, false)]
        [DataRow("2018-10-04 16:16:00", "2018-10-04 16:17:10", null, 1, 15, true)]
        [DataRow("2018-10-04 16:16:00", "2018-10-04 16:17:10", "SMS", null, 15, true)]
        [DataRow("2018-10-04 16:16:00", "2018-10-04 16:17:10", "SMS", 1, null, true)]
        public async Task Given_Parameters_When_ListMessagesStatus_Invoked_Then_It_Should_Return_Result(string startUpdateDate, string endUpdatedate, string? messageType, int? pageNum, int? pageSize, bool expected)
        {
            // Arrange
            ListMessageStatusRequestQueries queries = new ListMessageStatusRequestQueries()
            {
                StartUpdateDate = startUpdateDate,
                EndUpdateDate = endUpdatedate,
                MessageType = messageType,
                PageNumber = (pageNum != null) ? pageNum : 1,
                PageSize = (pageSize != null) ? pageSize : 15,
            };
            

            var requestUrl = new RequestUrlBuilder()
               .WithSettings(this._settings, this._settings.Endpoints.ListMessageStatus)
               .WithHeaders(this._headers).WithQueries(queries)
               .Build();

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