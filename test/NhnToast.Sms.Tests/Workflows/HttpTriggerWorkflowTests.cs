using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Aliencube.AzureFunctions.Extensions.Common;

using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;
using Toast.Common.Configurations;
using Toast.Common.Exceptions;
using Toast.Common.Models;
using Toast.Tests.Common.Fakes;
using Toast.Sms.Configurations;
using Toast.Sms.Triggers;
using Toast.Sms.Workflows;
using Toast.Sms.Models;
using System.Net.Http;
using FluentValidation;

using WorldDomination.Net.Http;
using Toast.Tests.Common.Configurations;
using Toast.Sms.Tests.Configurations;
using Toast.Common.Builders;

namespace Toast.Sms.Tests.Workflows
{
    [TestClass]
    public class HttpTriggerWorkflowTests

    {

        private Mock<IHttpClientFactory> _factory;
        private Mock<MediaTypeFormatter> _fomatter;
         [TestInitialize]
        public void Init()
        {
            this._factory = new Mock<IHttpClientFactory>();
            this._fomatter = new Mock<MediaTypeFormatter>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            this._factory = null;
            this._fomatter = null;
        }
        [TestMethod]
        public void Given_Type_When_Initiated_Then_It_Should_Implement_Interface()
        {
            var workflow = new HttpTriggerWorkflow(this._factory.Object, this._fomatter.Object);

            var hasInterface = workflow.GetType().HasInterface<IHttpTriggerWorkflow>();

            hasInterface.Should().BeTrue();
        }


        // //invoke 예외
        // [TestMethod]
        // public void Given_GetMessageResponse_Invoke_Then_It_Should_Throw_Exception(string method)
        // {
        //     // var headers = new HeaderDictionary();
        //     // headers.Add("Authorization", "Basic");

        //     // var req = new Mock<HttpRequest>();
        //     // req.SetupGet(p => p.Headers).Returns(headers);

        //     // var http = new HttpClient();
        //     // string requestUrl;
        //     var httpClient = new HttpClient();

        //     var factory = new Mock<IHttpClientFactory>();
        //     factory.Setup(p => p.CreateClient(It.IsAny<string>())).Returns(httpClient);
        //     var fomatter = new Mock<MediaTypeFormatter>();
        //     fomatter.Setup(p => p.CanWriteType(It.IsAny<Type>())).Returns(false);
        //     var workflow = new HttpTriggerWorkflow(factory.Object,fomatter.Object);
        //     // var result = await workflow.Invoke<GetMessageResponse>();
            
        //     Func<Task> func = async () => await workflow.InvokeAsync<GetMessageResponse>(new HttpMethod(method)).ConfigureAwait(false);


        //     func.Should().BeOfType<HttpTriggerWorkflow>();
        //     // func.Should().ThrowAsync<RequestHeaderNotValidException>();
        //     // func.Should().ThrowAsync<RequestBodyNotValidException>();
        //     // func.Should().ThrowAsync<RequestHeaderNotValidException>();
        // }

        
        // [DataTestMethod]
        // [DataRow(HttpVerbs.POST, HttpStatusCode.OK, true, 200, "hello world", "lorem ipsum")]
        // public async Task Given_Payload_When_Invoke_InvokeAsync_Then_It_Should_Return_Result(string method, HttpStatusCode statusCode, bool isSuccessful, int resultCode, string resultMessage, string body)
        // {
        //     var model = new FakeResponseModel()
        //     {
        //         Header = new ResponseHeaderModel()
        //         {
        //             IsSuccessful = isSuccessful,
        //             ResultCode = resultCode,
        //             ResultMessage = resultMessage
        //         },
        //         Body = body
        //     };
        //     var content = new ObjectContent<FakeResponseModel>(model, new JsonMediaTypeFormatter(), MediaTypeNames.Application.Json);
        //     var options = new HttpMessageOptions()
        //     {
        //         HttpResponseMessage = new HttpResponseMessage(statusCode) { Content = content }
        //     };

        //     var handler = new FakeHttpMessageHandler(options);

        //     var http = new HttpClient(handler);
        //     this._factory.Setup(p => p.CreateClient(It.IsAny<string>())).Returns(http);

        //     var workflow = new HttpTriggerWorkflow(this._factory.Object, this._fomatter.Object);

        //     var header = new RequestHeaderModel() { AppKey = "hello", SecretKey = "world" };
        //     var headers = typeof(HttpTriggerWorkflow).GetField("_headers", BindingFlags.Instance | BindingFlags.NonPublic);
        //     headers.SetValue(workflow, header);

        //     var url = "http://localhost:7071/api/HttpTrigger";
        //     var requestUrl = typeof(HttpTriggerWorkflow).GetField("_requestUrl", BindingFlags.Instance | BindingFlags.NonPublic);
        //     requestUrl.SetValue(workflow, url);

        //     var load = new FakeRequestModel()
        //     {
        //         FakeProperty1 = "lorem ipsum"
        //     };
        //     var payload = typeof(HttpTriggerWorkflow).GetField("_payload", BindingFlags.Instance | BindingFlags.NonPublic);
        //     payload.SetValue(workflow, load);

        //     var result = await workflow.InvokeAsync<FakeResponseModel>(new HttpMethod(method)).ConfigureAwait(false);

        //     result.Header.IsSuccessful.Should().Be(isSuccessful);
        //     result.Header.ResultCode.Should().Be(resultCode);
        //     result.Header.ResultMessage.Should().Be(resultMessage);
        //     result.Body.Should().Be(body);
        // }
        
    }
}