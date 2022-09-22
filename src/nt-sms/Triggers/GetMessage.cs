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
using Toast.Common.Exceptions;
using Toast.Common.Extensions;
using Toast.Common.Models;
using Toast.Common.Validators;
using Toast.Sms.Configurations;
using Toast.Sms.Examples;
using Toast.Sms.Models;
using Toast.Sms.Validators;
using Toast.Sms.Workflows;

namespace Toast.Sms.Triggers
{
    public class GetMessage
    {
        private readonly ToastSettings<SmsEndpointSettings> _settings;
        private readonly IHttpTriggerWorkflow _workflow;
        private readonly IValidator<GetMessageRequestQueries> _validator;
        private readonly ILogger<GetMessage> _logger;

        public GetMessage(ToastSettings<SmsEndpointSettings> settings, IHttpTriggerWorkflow workflow, IValidator<GetMessageRequestQueries> validator, ILogger<GetMessage> log)
        {
            this._settings = settings.ThrowIfNullOrDefault();
            this._workflow = workflow.ThrowIfNullOrDefault();
            this._validator = validator.ThrowIfNullOrDefault();
            this._logger = log.ThrowIfNullOrDefault();
        }

        [FunctionName(nameof(GetMessage))]
        [OpenApiOperation(operationId: "Messages.Get", tags: new[] { "messages" })]
        [OpenApiSecurity("app_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Basic, Description = "Toast API basic auth")]
        // [OpenApiSecurity("app_key", SecuritySchemeType.ApiKey, Name = "x-app-key", In = OpenApiSecurityLocationType.Header, Description = "Toast app key")]
        // [OpenApiSecurity("secret_key", SecuritySchemeType.ApiKey, Name = "x-secret-key", In = OpenApiSecurityLocationType.Header, Description = "Toast secret key")]
        // [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header, Description = "Functions API key")]
        [OpenApiParameter(name: "requestId", Type = typeof(string), In = ParameterLocation.Path, Required = true, Description = "SMS request ID")]
        [OpenApiParameter(name: "recipientSeq", Type = typeof(int), In = ParameterLocation.Query, Required = true, Description = "SMS request sequence number")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(GetMessageResponse), Example = typeof(GetMessageResponseModelExample), Description = "The OK response")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "The input was invalid")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.InternalServerError, Description = "The service has got an unexpected error")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "messages/{requestId:regex(^\\d+\\w+$)}")] HttpRequest req,
            string requestId)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            try
            {
                var paths = new GetMessageRequestPaths() {RequestId = requestId};
                var payload = await this._workflow.ValidateHeaderAsync(req)
                                                  .ValidateQueriesAsync(req, this._validator)
                                                  .BuildRequestUrlAsync(this._settings.Endpoints.GetMessage, this._settings, paths)
                                                  .InvokeAsync<GetMessageResponse>(HttpMethod.Get)
                                                  .ConfigureAwait(false);
                return new OkObjectResult(payload);
            }
            catch (ToastException ex)
            {
                var error = new ErrorMessageResponse();
                error.Header.IsSuccessful = false;
                error.Header.ResultCode = (int)ex.StatusCode;
                error.Header.ResultMessage = ex.Message;

                return new OkObjectResult(error);
            }
            catch (Exception ex)
            {
                var error = new ErrorMessageResponse();
                error.Header.IsSuccessful = false;
                error.Header.ResultCode = (int)HttpStatusCode.InternalServerError;
                error.Header.ResultMessage = ex.Message;

                return new OkObjectResult(error);
            }
        }
    }
}