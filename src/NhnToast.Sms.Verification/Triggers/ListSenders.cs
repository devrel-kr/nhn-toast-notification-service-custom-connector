using System;
using System.Net;
using System.Net.Http;
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

using Toast.Common.Configurations;
using Toast.Common.Models;
using Toast.Common.Validators;
using Toast.Sms.Verification.Configurations;

namespace Toast.Sms.Verification.Triggers
{
    public class ListSenders
    {
        private readonly ToastSettings<SmsVerificationEndpointSettings> _settings;
        private readonly HttpClient _http;
        private readonly ILogger<ListSenders> _logger;

        public ListSenders(ToastSettings<SmsVerificationEndpointSettings> settings, IHttpClientFactory factory, ILogger<ListSenders> log)
        {
            this._settings = settings.ThrowIfNullOrDefault();
            this._http = factory.ThrowIfNullOrDefault().CreateClient("senders");
            this._logger = log.ThrowIfNullOrDefault();
        }

        [FunctionName(nameof(ListSenders))]
        [OpenApiOperation(operationId: "Senders.List", tags: new[] { "senders" })]
        [OpenApiSecurity("app_key", SecuritySchemeType.ApiKey, Name = "x-app-key", In = OpenApiSecurityLocationType.Header, Description = "Toast app key")]
        [OpenApiSecurity("secret_key", SecuritySchemeType.ApiKey, Name = "x-secret-key", In = OpenApiSecurityLocationType.Header, Description = "Toast secret key")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header, Description = "Functions API key")]
        [OpenApiParameter(name: "sendNo", Type = typeof(string), In = ParameterLocation.Query, Required = false, Description = "Sender's phone number")]
        [OpenApiParameter(name: "useYn", Type = typeof(string), In = ParameterLocation.Query, Required = false, Description = "Value indicating whether the number is used or not")]
        [OpenApiParameter(name: "blockYn", Type = typeof(string), In = ParameterLocation.Query, Required = false, Description = "Value indicating whether the number is blocked or not")]
        [OpenApiParameter(name: "pageNum", Type = typeof(string), In = ParameterLocation.Query, Required = false, Description = "Page number in the pagination. Default value is '1'")]
        [OpenApiParameter(name: "pageSize", Type = typeof(string), In = ParameterLocation.Query, Required = false, Description = "Page size in the pagination. Default value is '15'")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(object), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "senders")] HttpRequest req)
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

            var appKey = this._settings.AppKey;
            var secretKey = this._settings.SecretKey;
            var baseUrl = this._settings.BaseUrl;
            var version = this._settings.Version;
            var endpoint = this._settings.Endpoints.ListSenders;
            var options = new
            {
                Version = version,
                AppKey = appKey,
                SendNo = req.Query["sendNo"].ToString(),
                UseYn = req.Query["useYn"].ToString(),
                BlockYn = req.Query["blockYn"].ToString(),
                PageNum = req.Query["pageNum"].ToString().IsNullOrWhiteSpace() ? "1" : req.Query["pageNum"].ToString(),
                PageSize = req.Query["pageSize"].ToString().IsNullOrWhiteSpace() ? "15" : req.Query["pageSize"].ToString()
            };
            var requestUrl = this._settings.Formatter.Format($"{baseUrl.TrimEnd('/')}/{endpoint.TrimStart('/')}", options);

            this._http.DefaultRequestHeaders.Add("X-Secret-Key", secretKey);
            var result = await this._http.GetAsync(requestUrl).ConfigureAwait(false);

            var payload = await result.Content.ReadAsAsync<object>().ConfigureAwait(false);

            return new OkObjectResult(payload);
        }
    }
}