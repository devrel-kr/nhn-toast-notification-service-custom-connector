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

namespace Toast.Sms.Verification.Triggers
{
    public class ListSenders
    {
        private readonly ILogger<ListSenders> _logger;

        public ListSenders(ILogger<ListSenders> log)
        {
            _logger = log;
        }

        [FunctionName(nameof(ListSenders))]
        [OpenApiOperation(operationId: "Sender.List", tags: new[] { "sender" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiParameter(name: "sendNo", Type = typeof(string), In = ParameterLocation.Query, Required = false, Description = "전송번호")]
        [OpenApiParameter(name: "useYn", Type = typeof(string), In = ParameterLocation.Query, Required = false, Description = "Use Y/N")]
        [OpenApiParameter(name: "blockYn", Type = typeof(string), In = ParameterLocation.Query, Required = false, Description = "Block Y/N")]
        [OpenApiParameter(name: "pageNum", Type = typeof(string), In = ParameterLocation.Query, Required = false, Description = "페이지 번호")]
        [OpenApiParameter(name: "pageSize", Type = typeof(string), In = ParameterLocation.Query, Required = false, Description = "페이지 크기")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(object), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "senders")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var appKey = Environment.GetEnvironmentVariable("Toast__AppKey");
            var secretKey = Environment.GetEnvironmentVariable("Toast__SecretKey");
            var baseUrl = Environment.GetEnvironmentVariable("Toast__BaseUrl");
            var version = Environment.GetEnvironmentVariable("Toast__Version");
            var endpoint = Environment.GetEnvironmentVariable("Toast__Endpoints__ListSenders");
            var options = new
            {
                version = version,
                appKey = appKey,
                sendNo = req.Query["sendNo"].ToString(),
                useYn = req.Query["useYn"].ToString(),
                blockYn = req.Query["blockYn"].ToString(),
                pageNum = req.Query["pageNum"].ToString().IsNullOrWhiteSpace() ? "1" : req.Query["pageNum"].ToString(),
                pageSize = req.Query["pageSize"].ToString().IsNullOrWhiteSpace() ? "15" : req.Query["pageSize"].ToString()
            };
            var requestUrl = Smart.Format($"{baseUrl.TrimEnd('/')}/{endpoint.TrimStart('/')}", options);

            var http = new HttpClient();

            // Act
            http.DefaultRequestHeaders.Add("X-Secret-Key", secretKey);
            var result = await http.GetAsync(requestUrl).ConfigureAwait(false);

            var payload = await result.Content.ReadAsAsync<object>().ConfigureAwait(false);


            return new OkObjectResult(payload);
        }
    }
}