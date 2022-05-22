using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

using Aliencube.AzureFunctions.Extensions.Common;

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

using Toast.Common.Builders;
using Toast.Common.Configurations;
using Toast.Common.Models;
using Toast.Common.Validators;
using Toast.Sms.Configurations;

namespace Toast.Sms.Triggers
{
    public class SendMessages
    {
        private readonly ToastSettings<SmsEndpointSettings> _settings;
        private readonly HttpClient _http;
        private readonly ILogger<SendMessages> _logger;

        public SendMessages(ToastSettings<SmsEndpointSettings> settings, IHttpClientFactory factory, ILogger<SendMessages> log)
        {
            this._settings = settings.ThrowIfNullOrDefault();
            this._http = factory.ThrowIfNullOrDefault().CreateClient("senders");
            this._logger = log.ThrowIfNullOrDefault();
        }

        [FunctionName(nameof(SendMessages))]
        [OpenApiOperation(operationId: "Messages.Send", tags: new[] { "messages" })]
        [OpenApiSecurity("app_key", SecuritySchemeType.ApiKey, Name = "x-app-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("secret_key", SecuritySchemeType.ApiKey, Name = "x-secret-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(object), Description = "Message payload to send")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(object), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "POST", Route = "messages")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var headers = default(RequestHeaderModel);
            try
            {
                headers = await req.To<RequestHeaderModel>(SourceFrom.Header).Validate().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return new BadRequestResult();
            }

            var data = default(object);
            using (var reader = new StreamReader(req.Body))
            {
                var json = await reader.ReadToEndAsync().ConfigureAwait(false);
                data = JsonConvert.DeserializeObject<object>(json);
            }

            var requestUrl = new RequestUrlBuilder()
                .WithSettings(this._settings, this._settings.Endpoints.SendMessages)
                .WithHeaders(headers)
                .Build();

            var content = new ObjectContent<object>(data, new JsonMediaTypeFormatter(), "application/json");

            // Act
            this._http.DefaultRequestHeaders.Add("X-Secret-Key", headers.SecretKey);
            var result = await this._http.PostAsync(requestUrl, content).ConfigureAwait(false);

            var payload = await result.Content.ReadAsAsync<object>().ConfigureAwait(false);

            return new OkObjectResult(payload);
        }
    }
}