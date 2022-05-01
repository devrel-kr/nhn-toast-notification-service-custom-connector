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

using Toast.Common.Configurations;
using Toast.Sms.Configurations;
using Toast.Sms.Models;

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
        [OpenApiSecurity("app_key", SecuritySchemeType.ApiKey, Name = "x-app-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("secret_key", SecuritySchemeType.ApiKey, Name = "x-secret-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
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

            var appKey = req.Headers["x-app-key"].ToString();
            var secretKey = req.Headers["x-secret-key"].ToString();
            var baseUrl = this._settings.BaseUrl;
            var version = this._settings.Version;
            var endpoint = this._settings.Endpoints.GetMessage;
            var options = new ListMessageStatusRequestUrlOptions()
            {
                Version = version,
                AppKey = appKey,
                StartUpdateDate = req.Query["startUpdateDate"].ToString(),
                EndUpdateDate = req.Query["endUpdateDate"].ToString(),
                MessageType = req.Query["messageType"].ToString(),
                PageNum = int.TryParse(req.Query["pageNum"].ToString(), out int pageNumVal) ? pageNumVal : 1,
                PageSize = int.TryParse(req.Query["pageSize"].ToString(), out int pageSizeVal) ? pageSizeVal : 15,
            };
            var requestUrl = this._settings.Formatter.Format($"{baseUrl.TrimEnd('/')}/{endpoint.TrimStart('/')}", options);

            this._http.DefaultRequestHeaders.Add("X-Secret-Key", secretKey);
            var result = await this._http.GetAsync(requestUrl).ConfigureAwait(false);

            dynamic payload = await result.Content.ReadAsAsync<object>().ConfigureAwait(false);

            return new OkObjectResult(payload);
        }
    }
}