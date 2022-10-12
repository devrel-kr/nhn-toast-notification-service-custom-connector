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
//using Toast.Tests.Common.Fakes;
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

         [TestInitialize]
        public void Init()
        {
            this._factory = new Mock<IHttpClientFactory>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            this._factory = null;

        }
        [TestMethod]
        public void Given_Type_When_Initiated_Then_It_Should_Implement_Interface()
        {
            var workflow = new HttpTriggerWorkflow(this._factory.Object);

            var hasInterface = workflow.GetType().HasInterface<IHttpTriggerWorkflow>();

            hasInterface.Should().BeTrue();
        }
        
        /*
        //쿼리 예외 테스트
        [TestMethod]
        public void Given_ValidQueries_fails_When_Invoke_ValidateQueriesAsync_Then_It_Should_Throw_Exception()
        {
            var queries = new QueryCollection();
            
            var req = new Mock<HttpRequest>();
            req.SetupGet(p => p.Query).Returns(queries);
            
            var validator = new Mock<IValidator<GetMessageRequestQueries>>();
            var workflow = new HttpTriggerWorkflow(this._factory.Object);
            Func<Task> func = async () => await workflow.ValidateQueriesAsync<GetMessageRequestQueries>(req.Object, validator.Object);

            func.Should().ThrowAsync<RequestQueryNotValidException>();
        }

        //쿼리 테스트
        [DataTestMethod]
        [DataRow("Hello","world")]
        public async Task Given_ValidQueries_When_Invoke_ValidateQueriesAsync_Then_It_Should_Return_Result(string name, string value)
        {

            var queries = new QueryCollection();

            var req = new Mock<HttpRequest>();
            req.SetupGet(p => p.Query).Returns(queries);

            var validator = new Mock<IValidator<GetMessageRequestQueries>>();
            var workflow = new HttpTriggerWorkflow(this._factory.Object);

            await workflow.ValidateQueriesAsync<GetMessageRequestQueries>(req.Object, validator.Object);

            var fi = workflow.GetType().GetField("_queries", BindingFlags.NonPublic | BindingFlags.Instance);
            var result = (GetMessageRequestQueries)fi.GetValue(workflow);

            result.Should().BeOfType<GetMessageRequestQueries>();

            //result.PropertyA.Should().Be(expected);
        }             
        */

    }
}