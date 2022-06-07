using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

using Aliencube.AzureFunctions.Extensions.Common;

using FluentValidation;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

using Toast.Common.Builders;
using Toast.Common.Configurations;
using Toast.Common.Extensions;
using Toast.Common.Models;
using Toast.Common.Validators;
using Toast.Sms.Configurations;
using Toast.Sms.Models;
using Toast.Sms.Validators;

namespace Toast.Sms.Triggers
{
    public class SendMessages
    {
        private readonly ToastSettings<SmsEndpointSettings> _settings;
        private readonly IValidator<SendMessagesRequestBody> _validator;
        private readonly HttpClient _http;
        private readonly ILogger<SendMessages> _logger;

        public SendMessages(ToastSettings<SmsEndpointSettings> settings, IValidator<SendMessagesRequestBody> validator, IHttpClientFactory factory, ILogger<SendMessages> log)
        {
            this._settings = settings.ThrowIfNullOrDefault();
            this._validator = validator.ThrowIfNullOrDefault();
            this._http = factory.ThrowIfNullOrDefault().CreateClient("senders");
            this._logger = log.ThrowIfNullOrDefault();
        }

        [FunctionName(nameof(SendMessages))]
        [OpenApiOperation(operationId: "Messages.Send", tags: new[] { "messages" })]
        [OpenApiSecurity("app_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Basic, Description = "Toast API basic auth")]
        // [OpenApiSecurity("app_key", SecuritySchemeType.ApiKey, Name = "x-app-key", In = OpenApiSecurityLocationType.Header)]
        // [OpenApiSecurity("secret_key", SecuritySchemeType.ApiKey, Name = "x-secret-key", In = OpenApiSecurityLocationType.Header)]
        // [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(SendMessagesRequestBody), Example = typeof(SendMessagesRequestBodyModelExample), Description = "Message payload to send")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(SendMessagesResponse),  Example = typeof(SendMessagesResponseModelExample), Description = "The OK response")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "The input was invalid")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.InternalServerError, Description = "The service has got an unexpected error")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "POST", Route = "messages")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var headers = default(RequestHeaderModel);
            try
            {
                headers = req.To<RequestHeaderModel>(useBasicAuthHeader: true).Validate();
                // headers = await req.To<RequestHeaderModel>(SourceFrom.Header).Validate().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new BadRequestResult();
            }

            var payload = default(SendMessagesRequestBody);
            try
            {
                payload = await req.To<SendMessagesRequestBody>(SourceFrom.Body).Validate(this._validator).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new BadRequestResult();
            }

            var requestUrl = new RequestUrlBuilder()
                .WithSettings(this._settings, this._settings.Endpoints.SendMessages)
                .WithHeaders(headers)
                .Build();

            var content = new ObjectContent<SendMessagesRequestBody>(payload, this._settings.JsonFormatter, "application/json");

            this._http.DefaultRequestHeaders.Add("X-Secret-Key", headers.SecretKey);
            var result = await this._http.PostAsync(requestUrl, content).ConfigureAwait(false);

            var resultPayload = await result.Content.ReadAsAsync<SendMessagesResponse>().ConfigureAwait(false);

            return new OkObjectResult(resultPayload);
        }
    }
}