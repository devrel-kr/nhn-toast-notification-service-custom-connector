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

using Toast.Common.Configurations;
using Toast.Common.Extensions;
using Toast.Common.Models;
using Toast.Common.Validators;
using Toast.Sms.Verification.Configurations;
using Toast.Sms.Verification.Models;
using Toast.Sms.Verification.Validators;

namespace Toast.Sms.Verification.Triggers
{
    /// <summary>
    /// This represents the HTTP trigger entity to get the list of sender's phone numbers.
    /// </summary>
    public class ListSenders
    {
        private readonly ToastSettings<SmsVerificationEndpointSettings> _settings;
        private readonly IValidator<ListSendersRequestQueries> _validator;
        private readonly HttpClient _http;
        private readonly ILogger<ListSenders> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListSenders"/> class.
        /// </summary>
        /// <param name="settings"><see cref="ToastSettings{T}"/> instance.</param>
        /// <param name="validator"><see cref="IValidator{T}"/> instance.</param>
        /// <param name="factory"><see cref="IHttpClientFactory"/> instance.</param>
        /// <param name="log"><see cref="ILogger{TCategoryName}"/> instance.</param>
        public ListSenders(ToastSettings<SmsVerificationEndpointSettings> settings, IValidator<ListSendersRequestQueries> validator, IHttpClientFactory factory, ILogger<ListSenders> log)
        {
            this._settings = settings.ThrowIfNullOrDefault();
            this._validator = validator.ThrowIfNullOrDefault();
            this._http = factory.ThrowIfNullOrDefault().CreateClient("senders");
            this._logger = log.ThrowIfNullOrDefault();
        }

        /// <summary>
        /// Invokes the endpoint to get the list of sender's phone numbers.
        /// </summary>
        /// <param name="req"><see cref="HttpRequest"/> instance.</param>
        /// <returns>Returns the <see cref="IActionResult"/> instance that contains the list of sender's phone numbers.</returns>
        [FunctionName(nameof(ListSenders))]
        [OpenApiOperation(operationId: "Senders.List", tags: new[] { "senders" })]
        [OpenApiSecurity("app_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Basic, Description = "Toast API basic auth")]
        // [OpenApiSecurity("app_key", SecuritySchemeType.ApiKey, Name = "x-app-key", In = OpenApiSecurityLocationType.Header, Description = "Toast app key")]
        // [OpenApiSecurity("secret_key", SecuritySchemeType.ApiKey, Name = "x-secret-key", In = OpenApiSecurityLocationType.Header, Description = "Toast secret key")]
        // [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header, Description = "Functions API key")]
        [OpenApiParameter(name: "sendNo", Type = typeof(string), In = ParameterLocation.Query, Required = false, Description = "Sender's phone number")]
        [OpenApiParameter(name: "useYn", Type = typeof(string), In = ParameterLocation.Query, Required = false, Description = "Value indicating whether the number is used or not")]
        [OpenApiParameter(name: "blockYn", Type = typeof(string), In = ParameterLocation.Query, Required = false, Description = "Value indicating whether the number is blocked or not")]
        [OpenApiParameter(name: "pageNum", Type = typeof(string), In = ParameterLocation.Query, Required = false, Description = "Page number in the pagination. Default value is '1'")]
        [OpenApiParameter(name: "pageSize", Type = typeof(string), In = ParameterLocation.Query, Required = false, Description = "Page size in the pagination. Default value is '15'")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(object), Description = "The OK response")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "The input was invalid")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.InternalServerError, Description = "The service has got an unexpected error")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "senders")] HttpRequest req)
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
                return new BadRequestResult();
            }

            var queries = default(ListSendersRequestQueries);
            try
            {
                queries = await req.To<ListSendersRequestQueries>(SourceFrom.Query).Validate(this._validator).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return new BadRequestResult();
            }

            var options = new ListSendersRequestUrlOptions()
            {
                Version = this._settings.Version,
                AppKey = headers.AppKey,
                SendNo = queries.SenderNumber,
                UseYn = queries.UseNumber,
                BlockYn = queries.BlockedNumber,
                PageNum = queries.PageNumber,
                PageSize = queries.PageSize,
            };
            var requestUrl = this._settings.Formatter.Format($"{this._settings.BaseUrl.TrimEnd('/')}/{this._settings.Endpoints.ListSenders.TrimStart('/')}", options);

            this._http.DefaultRequestHeaders.Add("X-Secret-Key", headers.SecretKey);
            var result = await this._http.GetAsync(requestUrl).ConfigureAwait(false);

            var payload = await result.Content.ReadAsAsync<object>().ConfigureAwait(false);

            return new OkObjectResult(payload);
        }
    }
}