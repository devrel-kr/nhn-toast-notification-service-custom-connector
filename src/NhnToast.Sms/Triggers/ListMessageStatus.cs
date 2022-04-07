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

namespace Toast.Sms.Triggers
{
    public class ListMessageStatus
    {
        private readonly ILogger<ListMessageStatus> _logger;

        public ListMessageStatus(ILogger<ListMessageStatus> log)
        {
            _logger = log;
        }

        [FunctionName(nameof(ListMessageStatus))]
        [OpenApiOperation(operationId: "MessageStatus.List", tags: new[] { "messageStatus" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiParameter(name: "startUpdateDate", Type = typeof(string), In = ParameterLocation.Query, Required = true, Description = "StartDate for message list(yyyy-MM-dd HH:mm:ss)")]
        [OpenApiParameter(name: "endUpdateDate", Type = typeof(string), In = ParameterLocation.Query, Required = true, Description = "endDate for message list(yyyy-MM-dd HH:mm:ss)")]
        [OpenApiParameter(name: "messageType", Type = typeof(string), In = ParameterLocation.Query, Required = false, Description = "message type(SMS/LMS/MMS/AUTH)")]
        [OpenApiParameter(name: "pageNum", Type = typeof(string), In = ParameterLocation.Query, Required = false, Description = "Page number in the pagination. Default value is '1'")]
        [OpenApiParameter(name: "pageSize", Type = typeof(string), In = ParameterLocation.Query, Required = false, Description = "Page size in the pagination. Default value is '15'")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "messages/status")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var appKey = Environment.GetEnvironmentVariable("Toast__AppKey");
            var secretKey = Environment.GetEnvironmentVariable("Toast__SecretKey");
            var baseUrl = Environment.GetEnvironmentVariable("Toast__BaseUrl");
            var version = Environment.GetEnvironmentVariable("Toast__Version");
            var endpoint = Environment.GetEnvironmentVariable("Toast__Endpoints__ListMessageStatus");
            var options = new
            {
                version = version,
                appKey = appKey,
                startUpdateDate = req.Query["startUpdateDate"].ToString(),
                endUpdateDate = req.Query["endUpdateDate"].ToString(),
                messageType = req.Query["messageType"].ToString(),
                pageNum = req.Query["pageNum"].ToString().IsNullOrWhiteSpace() ? "1" : req.Query["pageNum"].ToString(),
                pageSize = req.Query["pageSize"].ToString().IsNullOrWhiteSpace() ? "15" : req.Query["pageSize"].ToString() 
            };
            var requestUrl = Smart.Format($"{baseUrl.TrimEnd('/')}/{endpoint.TrimStart('/')}", options);

            var http = new HttpClient();

            // Act
            http.DefaultRequestHeaders.Add("X-Secret-Key", secretKey);
            var result = await http.GetAsync(requestUrl).ConfigureAwait(false);

            dynamic payload = await result.Content.ReadAsAsync<object>().ConfigureAwait(false);

            return new OkObjectResult(payload);
        }
    }
}