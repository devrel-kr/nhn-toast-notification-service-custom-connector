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
    public class ListMessagesTests
    {
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
        [DataRow(true, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, false)]
        [DataRow(true, null, null, null, null, null, null, null, null, null, null, null, null, null, null, 1, null, false)]
        [DataRow(true, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, 15, false)]
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
            var config = new ConfigurationBuilder().AddJsonFile("test.settings.json").Build();
            var appKey = config.GetValue<string>("Toast:AppKey");
            var secretKey = config.GetValue<string>("Toast:SecretKey");
            var baseUrl = config.GetValue<string>("Toast:BaseUrl");
            var version = config.GetValue<string>("Toast:Version");
            var endpoint = config.GetValue<string>("Toast:Endpoints:ListMessages");
            var requestId = useRequestId ? config.GetValue<string>("Toast:Examples:RequestId") : null;
            var options = new ListMessagesOptions()
            {
                Version = version,
                AppKey = appKey,
                RequestId = requestId,
                StartRequestDate = startRequestDate,
                EndRequestDate = endRequestDate,
                StartCreateDate = startCreateDate,
                EndCreateDate = endCreateDate,
                StartResultDate = startResultDate,
                EndResultDate = endResultDate,
                SendNo = sendNo,
                RecipientNo = recipientNo,
                TemplateId = templateId,
                MsgStatus = msgStatus,
                ResultCode = resultCode,
                SubResultCode = subResultCode,
                SenderGroupingKey = senderGroupingKey,
                RecipientGroupingKey = recipientGroupingKey,
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