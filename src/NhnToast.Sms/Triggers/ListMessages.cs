using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

using Newtonsoft.Json;
using SmartFormat;
using Toast.Common.Configurations;
using Toast.Sms.Configurations;
using Toast.Sms.Models;

namespace Toast.Sms.Triggers
{
    public class ListMessages
    {
        private readonly ILogger<ListMessages> _logger;

        public ListMessages(ILogger<ListMessages> log)
        {
            _logger = log;
        }

        [FunctionName(nameof(ListMessages))]
        [OpenApiOperation(operationId: "Messages.List", tags: new[] { "messages" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiParameter(name: "requestId", Type = typeof(string), In = ParameterLocation.Query, Required = false, Description = "RequestId to search. `requestId` or `startRequestDate` + `endRequestDate` or `startCreateDate` + `endCreateDate` must be filled")]
        [OpenApiParameter(name: "startRequestDate", Type = typeof(string), In = ParameterLocation.Query, Required = false, Description = "Message sending request start date (`yyyy-MM-dd HH:mm:ss`)")]
        [OpenApiParameter(name: "endRequestDate", Type = typeof(string), In = ParameterLocation.Query, Required = false, Description = "Message sending request end date (`yyyy-MM-dd HH:mm:ss`)")]
        [OpenApiParameter(name: "startCreateDate", Type = typeof(string), In = ParameterLocation.Query, Required = false, Description = "Message sending registration start date (`yyyy-MM-dd HH:mm:ss`)")]
        [OpenApiParameter(name: "endCreateDate", Type = typeof(string), In = ParameterLocation.Query, Required = false, Description = "Message sending registration end date (`yyyy-MM-dd HH:mm:ss`)")]
        [OpenApiParameter(name: "startResultDate", Type = typeof(string), In = ParameterLocation.Query, Required = false, Description = "Message sending complete start date (`yyyy-MM-dd HH:mm:ss`)")]
        [OpenApiParameter(name: "endResultDate", Type = typeof(string), In = ParameterLocation.Query, Required = false, Description = "Message sending complete end date (`yyyy-MM-dd HH:mm:ss`)")]
        [OpenApiParameter(name: "sendNo", Type = typeof(string), In = ParameterLocation.Query, Required = false, Description = "Sender's phone number")]
        [OpenApiParameter(name: "recipientNo", Type = typeof(string), In = ParameterLocation.Query, Required = false, Description = "Receiver's phone number")]
        [OpenApiParameter(name: "templateId", Type = typeof(string), In = ParameterLocation.Query, Required = false, Description = "Template number")]
        [OpenApiParameter(name: "msgStatus", Type = typeof(string), In = ParameterLocation.Query, Required = false, Description = "Message status code (`0`: fail, `1`: request, `2`: processing, `3`: success, `4`: Reservation cancellation, `5`: Duplicate failed)")]
        [OpenApiParameter(name: "resultCode", Type = typeof(string), In = ParameterLocation.Query, Required = false, Description = "Receive result code (`MTR1`: success, `MTR2`: fail)")]
        [OpenApiParameter(name: "subResultCode", Type = typeof(string), In = ParameterLocation.Query, Required = false, Description = "Receive result detail code (`MTR2_1`: Validation failed, `MTR2_2`: carrier problem, `MTR2_3`: Device problem)")]
        [OpenApiParameter(name: "senderGroupingKey", Type = typeof(string), In = ParameterLocation.Query, Required = false, Description = "Sender's group key")]
        [OpenApiParameter(name: "recipientGroupingKey", Type = typeof(string), In = ParameterLocation.Query, Required = false, Description = "Receiver's group key")]
        [OpenApiParameter(name: "pageNum", Type = typeof(string), In = ParameterLocation.Query, Required = false, Description = "Page number in the pagination. Default value is '1'")]
        [OpenApiParameter(name: "pageSize", Type = typeof(string), In = ParameterLocation.Query, Required = false, Description = "Page size in the pagination. Default value is '15'")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(object), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "messages")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var appKey = Environment.GetEnvironmentVariable("Toast__AppKey");
            var secretKey = Environment.GetEnvironmentVariable("Toast__SecretKey");
            var baseUrl = Environment.GetEnvironmentVariable("Toast__BaseUrl");
            var version = Environment.GetEnvironmentVariable("Toast__Version");
            var endpoint = Environment.GetEnvironmentVariable("Toast__Endpoints__ListMessages");
            var options = new ListMessagesOptions()
            {
                Version = version,
                AppKey = appKey,
                RequestId = req.Query["requestId"].ToString(),
                StartRequestDate = req.Query["startRequestDate"].ToString(),
                EndRequestDate = req.Query["endRequestDate"].ToString(),
                StartCreateDate = req.Query["startCreateDate"].ToString(),
                EndCreateDate = req.Query["endCreateDate"].ToString(),
                StartResultDate = req.Query["startResultDate"].ToString(),
                EndResultDate = req.Query["endResultDate"].ToString(),
                SendNo = req.Query["sendNo"].ToString(),
                RecipientNo = req.Query["recipientNo"].ToString(),
                TemplateId = req.Query["templateId"].ToString(),
                MsgStatus = req.Query["msgStatus"].ToString(),
                ResultCode = req.Query["resultCode"].ToString(),
                SubResultCode = req.Query["subResultCode"].ToString(),
                SenderGroupingKey = req.Query["senderGroupingKey"].ToString(),
                RecipientGroupingKey = req.Query["recipientGroupingKey"].ToString(),
                PageNum = int.TryParse(req.Query["pageNum"].ToString(), out int pageNumParse) ? pageNumParse : 1,
                PageSize = int.TryParse(req.Query["pageSize"].ToString(), out int pageSizeParse) ? pageSizeParse : 15         
            };
            var requestUrl = Smart.Format($"{baseUrl.TrimEnd('/')}/{endpoint.TrimStart('/')}", options);

            var http = new HttpClient();

            http.DefaultRequestHeaders.Add("X-Secret-Key", secretKey);
            var result = await http.GetAsync(requestUrl).ConfigureAwait(false);

            var payload = await result.Content.ReadAsAsync<object>().ConfigureAwait(false);


            return new OkObjectResult(payload);
        }
    }
}