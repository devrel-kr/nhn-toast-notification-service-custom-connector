using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;

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
    public class SendMessages
    {
        private readonly ILogger<SendMessages> _logger;

        public SendMessages(ILogger<SendMessages> log)
        {
            _logger = log;
        }

        [FunctionName(nameof(SendMessages))]
        [OpenApiOperation(operationId: "Sender.List", tags: new[] { "sender" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiParameter(name: "body", Type = typeof(string), In = ParameterLocation.Query, Required = true, Description = "Body of Message to send")]
        [OpenApiParameter(name: "sendNo", Type = typeof(string), In = ParameterLocation.Query, Required = true, Description = "Message Sender's No")]
        [OpenApiParameter(name: "recipientNo", Type = typeof(string), In = ParameterLocation.Query, Required = true, Description = "Message Recipient's No")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(object), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "POST", Route = "messages")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var appKey = Environment.GetEnvironmentVariable("Toast__AppKey");
            var secretKey = Environment.GetEnvironmentVariable("Toast__SecretKey");
            var baseUrl = Environment.GetEnvironmentVariable("Toast__BaseUrl");
            var version = Environment.GetEnvironmentVariable("Toast__Version");
            var endpoint = Environment.GetEnvironmentVariable("Toast__Endpoints__SendMessages");
            var body = req.Query["body"].ToString();
            var sendNo = req.Query["sendNo"].ToString();
            var recipientNo = req.Query["recipientNo"].ToString();
            var options = new
            {
                version = version,
                appKey = appKey
            };

            var json = $"{{\"body\":\"{body}\",\"sendNo\":\"{sendNo}\",\"recipientList\":[{{\"recipientNo\":\"{recipientNo}\"}}]}}";
            var requestUrl = Smart.Format($"{baseUrl.TrimEnd('/')}/{endpoint.TrimStart('/')}", options);

            var http = new HttpClient();

            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            http.DefaultRequestHeaders.Add("X-Secret-Key", secretKey);
            var result = await http.PostAsync(requestUrl, content).ConfigureAwait(false);

            var payload = await result.Content.ReadAsAsync<object>().ConfigureAwait(false);


            return new OkObjectResult(payload);
        }
    }
}