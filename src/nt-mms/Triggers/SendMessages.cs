using System;
using System.Net;
using System.Net.Http;
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
using Toast.Mms.Configurations;
using Toast.Mms.Examples;
using Toast.Mms.Models;
using Toast.Mms.Validators;

namespace Toast.Mms.Triggers
{
    public class SendMessages
    {
        private readonly ToastSettings<MmsEndpointSettings> _settings;
        private readonly IValidator<SendMessagesRequestBody> _validator;
        private readonly HttpClient _http;
        private readonly ILogger<SendMessages> _logger;

        public SendMessages(ToastSettings<MmsEndpointSettings> settings, IValidator<SendMessagesRequestBody> validator, IHttpClientFactory factory, ILogger<SendMessages> log)
        {
            this._settings = settings.ThrowIfNullOrDefault();
            this._validator = validator.ThrowIfNullOrDefault();
            this._http = factory.ThrowIfNullOrDefault().CreateClient("mms");
            this._logger = log.ThrowIfNullOrDefault();
        }

        [FunctionName(nameof(SendMessages))]
        [OpenApiOperation(operationId: "MMS.Send", tags: new[] { "mms" })]
        [OpenApiSecurity(schemeName: "function_key", schemeType: SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header, Description = "API Key")]
        [OpenApiSecurity(schemeName: "app_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Basic, Description = "Toast API basic auth")]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(SendMessagesRequestBody), Example = typeof(SendMessagesRequestBodyModelExample), Description = "Message payload to send")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: ContentTypes.ApplicationJson, bodyType: typeof(SendMessagesResponseBody), Example = typeof(SendMessagesResponseModelExample), Description = "The OK response")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: ContentTypes.ApplicationJson, bodyType: typeof(ErrorResponseModel), Example = typeof(BadRequestResponseModelExample), Description = "The input was invalid")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError, contentType: ContentTypes.ApplicationJson, bodyType: typeof(ErrorResponseModel), Example = typeof(InternalServerErrorResponseModelExample), Description = "The service has got an unexpected error")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "POST", Route = "messages")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var headers = default(RequestHeaderModel);
            try
            {
                headers = req.To<RequestHeaderModel>(useBasicAuthHeader: true)
                             .Validate();
                //headers = await req.To<RequestHeaderModel>(SourceFrom.Header)
                //                   .Validate().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new BadRequestResult();
            }

            var payload = default(SendMessagesRequestBody);
            try
            {
                payload = await req.To<SendMessagesRequestBody>(SourceFrom.Body)
                                   .Validate(this._validator)
                                   .ConfigureAwait(false);
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

            var content = new ObjectContent<SendMessagesRequestBody>(payload, this._settings.JsonFormatter, ContentTypes.ApplicationJson);

            this._http.DefaultRequestHeaders.Add("X-Secret-Key", headers.SecretKey);
            try
            {
                var result = await this._http.PostAsync(requestUrl, content).ConfigureAwait(false);

                var resultPayload = await result.Content
                                                .ReadAsAsync<SendMessagesResponseBody>()
                                                .ConfigureAwait(false);

                return new OkObjectResult(resultPayload);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message);
                var resultPayload = new ErrorResponseModel()
                {
                    Message = "Something has gone wrong"
                };

                return new ObjectResult(resultPayload)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }
    }
}