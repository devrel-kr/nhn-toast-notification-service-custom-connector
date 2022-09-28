using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configurations.AppSettings.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Toast.Common.Builders;
using Toast.Common.Configurations;
using Toast.Common.Exceptions;
using Toast.Common.Models;
using Toast.Sms.Configurations;
using Toast.Sms.Models;
using Toast.Sms.Tests.Configurations;
using Toast.Sms.Triggers;
using Toast.Sms.Workflows;
using Toast.Tests.Common.Configurations;

namespace Toast.Sms.Tests.Triggers
{
    [TestClass]
    public class GetMessageTests
    {
        private RequestHeaderModel _headers;
        private ToastTestSettings<SmsEndpointSettings, SmsExamplesSettings> _settings;
        private readonly IHttpTriggerWorkflow _workflow;
        private readonly IValidator<GetMessageRequestQueries> _validator;
        private readonly ILogger<GetMessage> _logger;

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
        [DataRow(false, 1, false)]
        [DataRow(true, 1, true)]
        [DataRow(true, 100, true)]
        public async Task Given_Parameters_When_GetMessage_Invoked_Then_It_Should_Return_Result(bool useRequestId, int recipientSeq, bool expected)
        {
            // Arrange  
            GetMessageRequestQueries? queries = new GetMessageRequestQueries() { RecipientSequenceNumber = recipientSeq };
            
            var paths = new GetMessageRequestPaths() { RequestId = useRequestId ? this._settings.Examples.RequestId : null };
            var requestUrl = new RequestUrlBuilder()
                .WithSettings<ToastSettings>(this._settings, this._settings.Endpoints.GetMessage)
                .WithHeaders(this._headers)
                .WithQueries(queries)
                .WithPaths(paths)
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
    /*
        [DataTestMethod]
        [DataRow("name")]
        //[ExpectedException(typeof(ToastException))]
        public async Task Given_Exception_When_Invoke_ValidateHeaderAsync_Then_It_Should_Throw_ToastException (string requestId)
        {
            var req = new Mock<HttpRequest>();
            var workflow = new Mock<IHttpTriggerWorkflow>();

            workflow.Setup(p => p.ValidateHeaderAsync(It.IsAny<HttpRequest>())).Throws<RequestHeaderNotValidException>();

            var trigger = new GetMessage(this._settings, this._workflow, this._validator, this._logger);
            var result = await trigger.Run(req.Object, requestId).ConfigureAwait(false);
            
            //result.Should().BeOfType<ToastException>();
            //result.Should().Be(workflow);
            result.Should().BeOfType<OkObjectResult>();
            var objectResult = (OkObjectResult)result;
            objectResult.Value.Should().BeOfType<ErrorMessageResponse>();
        }
    */
    }
}