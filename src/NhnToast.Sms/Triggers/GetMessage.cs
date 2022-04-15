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

using Toast.Sms.Configurations;

namespace Toast.Sms.Triggers
{
    public class GetMessage
    {
        private readonly ToastSettings _settings;
        private readonly ILogger<GetMessage> _logger;

        public GetMessage(ToastSettings settings, ILogger<GetMessage> log)
        {
            this._settings = settings.ThrowIfNullOrDefault();
            this._logger = log.ThrowIfNullOrDefault();
        }

        [FunctionName(nameof(GetMessage))]
        [OpenApiOperation(operationId: "Messages.Get", tags: new[] { "messages" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiParameter(name: "requestId", Type = typeof(string), In = ParameterLocation.Path, Required = true, Description = "SMS request ID")]
        [OpenApiParameter(name: "recipientSeq", Type = typeof(string), In = ParameterLocation.Query, Required = true, Description = "SMS request sequence number")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(object), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "messages/{requestId:regex(^\\d+\\w+$)}")] HttpRequest req, string requestId)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            int.TryParse(req.Query["recipientSeq"].ToString(), out int recipientSeqVal);

            var appKey = Environment.GetEnvironmentVariable("Toast__AppKey");
            var secretKey = Environment.GetEnvironmentVariable("Toast__SecretKey");
            var baseUrl = Environment.GetEnvironmentVariable("Toast__BaseUrl");
            var version = Environment.GetEnvironmentVariable("Toast__Version");
            var endpoint = Environment.GetEnvironmentVariable("Toast__Endpoints__GetMessage");
            var options = new GetMessagRequestUrlOptions()
            {
                version = version,
                appKey = appKey,
                requestId = requestId,
                recipientSeq = recipientSeqVal,
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