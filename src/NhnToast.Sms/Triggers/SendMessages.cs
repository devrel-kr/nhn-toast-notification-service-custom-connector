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
using System.Net.Http.Formatting;

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
        [OpenApiOperation(operationId: "Message.Send", tags: new[] { "message" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(object), Description ="Message payload to send")]
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
            var options = new
            {
                version = version,
                appKey = appKey
            };

            var data = default(object);
            using (var reader  = new StreamReader(req.Body))
            {
                var json = await reader.ReadToEndAsync().ConfigureAwait(false);
                data = JsonConvert.DeserializeObject<object>(json);
            }

            var requestUrl = Smart.Format($"{baseUrl.TrimEnd('/')}/{endpoint.TrimStart('/')}", options);

            var http = new HttpClient();

            var content = new ObjectContent<object>(data, new JsonMediaTypeFormatter(), "application/json");

            // Act
            http.DefaultRequestHeaders.Add("X-Secret-Key", secretKey);
            var result = await http.PostAsync(requestUrl, content).ConfigureAwait(false);

            var payload = await result.Content.ReadAsAsync<object>().ConfigureAwait(false);


            return new OkObjectResult(payload);
        }
    }
}