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
using Toast.Sms.Bulder;
using Toast.Sms.Configurations;


namespace Toast.Sms.Triggers
{
    public class ListMessageStatus
    {
        private readonly ToastSettings<SmsEndpointSettings> _settings;
        private readonly HttpClient _http;
        private readonly ILogger<ListMessageStatus> _logger;

        public ListMessageStatus(ToastSettings<SmsEndpointSettings> settings, IHttpClientFactory factory, ILogger<ListMessageStatus> log)
        {
            this._settings = settings.ThrowIfNullOrDefault();
            this._http = factory.ThrowIfNullOrDefault().CreateClient("messages");
            this._logger = log.ThrowIfNullOrDefault();
        }

        [FunctionName(nameof(ListMessageStatus))]
        [OpenApiOperation(operationId: "Messages.Status", tags: new[] { "messages" })]
        [OpenApiSecurity("app_key", SecuritySchemeType.ApiKey, Name = "x-app-key", In = OpenApiSecurityLocationType.Header, Description = "Toast app key")]
        [OpenApiSecurity("secret_key", SecuritySchemeType.ApiKey, Name = "x-secret-key", In = OpenApiSecurityLocationType.Header, Description = "Toast secret key")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header, Description = "Functions API key")]
        [OpenApiParameter(name: "startUpdateDate", Type = typeof(string), In = ParameterLocation.Query, Required = true, Description = "StartDate for message list (`yyyy-MM-dd HH:mm:ss`)")]
        [OpenApiParameter(name: "endUpdateDate", Type = typeof(string), In = ParameterLocation.Query, Required = true, Description = "endDate for message list (`yyyy-MM-dd HH:mm:ss`)")]
        [OpenApiParameter(name: "messageType", Type = typeof(string), In = ParameterLocation.Query, Required = false, Description = "message type (`SMS`/`LMS`/`MMS`/`AUTH`)")]
        [OpenApiParameter(name: "pageNum", Type = typeof(string), In = ParameterLocation.Query, Required = false, Description = "Page number in the pagination. Default value is '1'")]
        [OpenApiParameter(name: "pageSize", Type = typeof(string), In = ParameterLocation.Query, Required = false, Description = "Page size in the pagination. Default value is '15'")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "messages/status")] HttpRequest req)
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

            var requestUrl = new ListMessagesRequestUrlBuilder().WithSettings(this._settings).WithHeaders(headers).WithQueries(req).Build();

            this._http.DefaultRequestHeaders.Add("X-Secret-Key", headers.SecretKey);
            var result = await this._http.GetAsync(requestUrl).ConfigureAwait(false);

            dynamic payload = await result.Content.ReadAsAsync<object>().ConfigureAwait(false);

            return new OkObjectResult(payload);
        }
    }
}