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
    public class ListMessagesTests
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
        [DataRow(false, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, false)]
        [DataRow(false, null, null, null, null, null, null, null, null, null, null, null, null, null, null, 1, null, false)]
        [DataRow(false, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, 15, false)]
        [DataRow(false, null, null, null, null, null, null, null, null, null, null, null, null, null, null, 1, 15, false)]
        [DataRow(false, "2022-03-22 18:00:00", null, null, null, null, null, null, null, null, null, null, null, null, null, 1, 15, false)]
        [DataRow(false, null, "2022-03-22 22:00:00", null, null, null, null, null, null, null, null, null, null, null, null, 1, 15, false)]
        [DataRow(false, null, null, "2022-03-22 18:00:00", null, null, null, null, null, null, null, null, null, null, null, 1, 15, false)]
        [DataRow(false, null, null, null, "2022-03-22 22:00:00", null, null, null, null, null, null, null, null, null, null, 1, 15, false)]
        [DataRow(false, "2022-03-22 18:00:00", null, "2022-03-22 18:00:00", null, null, null, null, null, null, null, null, null, null, null, 1, 15, false)]
        [DataRow(false, "2022-03-22 18:00:00", null, null, "2022-03-22 22:00:00", null, null, null, null, null, null, null, null, null, null, 1, 15, false)]
        [DataRow(false, null, "2022-03-22 22:00:00", "2022-03-22 18:00:00", null, null, null, null, null, null, null, null, null, null, null, 1, 15, false)]
        [DataRow(false, null, "2022-03-22 22:00:00", null, "2022-03-22 18:00:00", null, null, null, null, null, null, null, null, null, null, 1, 15, false)]
        [DataRow(true, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, true)]
        [DataRow(true, null, null, null, null, null, null, null, null, null, null, null, null, null, null, 1, null, true)]
        [DataRow(true, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, 15, true)]
        [DataRow(true, null, null, null, null, null, null, null, null, null, null, null, null, null, null, 1, 15, true)]
        [DataRow(true, null, null, "2022-03-22 22:00:00", "2022-03-22 18:00:00", null, null, null, null, null, null, null, null, null, null, 1, 15, true)]
        [DataRow(true, "2022-03-22 18:00:00", "2022-03-22 22:00:00", null, null, null, null, null, null, null, null, null, null, null, null, 1, 15, true)]
        [DataRow(true, "2022-03-22 18:00:00", null, null, null, null, null, null, null, null, null, null, null, null, null, 1, 15, true)]
        [DataRow(true, null, "2022-03-22 18:00:00", null, null, null, null, null, null, null, null, null, null, null, null, 1, 15, true)]
        [DataRow(true, null, null, "2022-03-22 18:00:00", null, null, null, null, null, null, null, null, null, null, null, 1, 15, true)]
        [DataRow(true, null, null, null, "2022-03-22 18:00:00", null, null, null, null, null, null, null, null, null, null, 1, 15, true)]
        [DataRow(true, "2022-03-22 18:00:00", "2022-03-22 18:00:00", null, null, null, null, null, null, null, null, null, null, null, null, 1, 15, true)]
        [DataRow(true, "2022-03-22 18:00:00", null, "2022-03-22 18:00:00", null, null, null, null, null, null, null, null, null, null, null, 1, 15, true)]
        [DataRow(true, "2022-03-22 18:00:00", null, null, "2022-03-22 22:00:00", null, null, null, null, null, null, null, null, null, null, 1, 15, true)]
        [DataRow(true, null, "2022-03-22 22:00:00", "2022-03-22 18:00:00", null, null, null, null, null, null, null, null, null, null, null, 1, 15, true)]
        [DataRow(true, null, "2022-03-22 18:00:00", null, "2022-03-22 18:00:00", null, null, null, null, null, null, null, null, null, null, 1, 15, true)]
        [DataRow(true, null, null, "2022-03-22 18:00:00", "2022-03-22 18:00:00", null, null, null, null, null, null, null, null, null, null, 1, 15, true)]
        [DataRow(true, "2022-03-22 18:00:00", "2022-03-22 18:00:00", "2022-03-22 18:00:00", null, null, null, null, null, null, null, null, null, null, null, 1, 15, true)]
        [DataRow(true, "2022-03-22 18:00:00", "2022-03-22 18:00:00", null, "2022-03-22 18:00:00", null, null, null, null, null, null, null, null, null, null, 1, 15, true)]
        [DataRow(true, "2022-03-22 18:00:00", null, "2022-03-22 18:00:00", "2022-03-22 18:00:00", null, null, null, null, null, null, null, null, null, null, 1, 15, true)]
        [DataRow(true, null, "2022-03-22 18:00:00", "2022-03-22 18:00:00", "2022-03-22 18:00:00", null, null, null, null, null, null, null, null, null, null, 1, 15, true)]
        [DataRow(true, "2022-03-22 18:00:00", "2022-03-22 18:00:00", "2022-03-22 18:00:00", "2022-03-22 18:00:00", null, null, null, null, null, null, null, null, null, null, 1, 15, true)]
        [DataRow(true, null, null, null, null, "2022-03-22 18:00:00", null, null, null, null, null, null, null, null, null, 1, 15, true)]
        [DataRow(true, null, null, null, null, null, "2022-03-22 18:00:00", null, null, null, null, null, null, null, null, 1, 15, true)]
        [DataRow(true, null, null, null, null, null, null, "01012345678", null, null, null, null, null, null, null, 1, 15, true)]
        [DataRow(true, null, null, null, null, null, null, null, "01012345678", null, null, null, null, null, null, 1, 15, true)]
        [DataRow(true, null, null, null, null, null, null, null, null, "testTemplateId", null, null, null, null, null, 1, 15, true)]
        [DataRow(true, null, null, null, null, null, null, null, null, null, "3", null, null, null, null, 1, 15, true)]
        [DataRow(true, null, null, null, null, null, null, null, null, null, null, "MTR1", null, null, null, 1, 15, true)]
        [DataRow(true, null, null, null, null, null, null, null, null, null, null, null, "MTR2_1", null, null, 1, 15, true)]
        [DataRow(true, null, null, null, null, null, null, null, null, null, null, null, null, "testGroupKey", null, 1, 15, true)]
        [DataRow(true, null, null, null, null, null, null, null, null, null, null, null, null, null, "testGroupKey", 1, 15, true)]
        public async Task Given_Parameters_When_ListMessages_Invoked_Then_It_Should_Return_Result(bool useRequestId, string startRequestDate, string endRequestDate, string startCreateDate, string endCreateDate,
            string startResultDate, string endResultDate, string sendNo, string recipientNo, string templateId, string msgStatus, string resultCode, string subResultCode, string senderGroupingKey, string recipientGroupingKey, int? pageNum, int? pageSize, bool expected)
        {
            // Arrange
            ListMessagesRequestQueries queries = new ListMessagesRequestQueries()
            {
                RequestId = useRequestId ? this._settings.Examples.RequestId : null,
                StartRequestDate = startRequestDate,
                EndRequestDate = endRequestDate,
                StartCreateDate = startCreateDate,
                EndCreateDate = endCreateDate,
                StartResultDate = startResultDate,
                EndResultDate = endResultDate,
                SendNumber = sendNo,
                RecipientNumber = recipientNo,
                TemplateId = templateId,
                MessageStatus = msgStatus,
                ResultCode = resultCode,
                SubResultCode = subResultCode,
                SenderGroupingKey = senderGroupingKey,
                RecipientGroupingKey = recipientGroupingKey,
                PageNumber = (pageNum != null) ? pageNum : 1,
                PageSize = (pageSize != null) ? pageSize : 15,
            };
            var requestUrl = new RequestUrlBuilder()
                .WithSettings(this._settings, this._settings.Endpoints.ListMessages)
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