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
using Toast.Sms.Configurations;
using Toast.Sms.Models;

namespace Toast.Sms.Triggers
{
    public class ListMessages
    {
        private readonly ToastSettings<SmsEndpointSettings> _settings;
        private readonly HttpClient _http;
        private readonly ILogger<ListMessages> _logger;

        public ListMessages(ToastSettings<SmsEndpointSettings> settings, IHttpClientFactory factory, ILogger<ListMessages> log)
        {
            this._settings = settings.ThrowIfNullOrDefault();
            this._http = factory.ThrowIfNullOrDefault().CreateClient("messages");
            this._logger = log.ThrowIfNullOrDefault();
        }

        [FunctionName(nameof(ListMessages))]
        [OpenApiOperation(operationId: "Messages.List", tags: new[] { "messages" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("app_key", SecuritySchemeType.ApiKey, Name = "x-app-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("secret_key", SecuritySchemeType.ApiKey, Name = "x-secret-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiParameter(name: "requestId", Type = typeof(string), In = ParameterLocation.Query, Required = false, Description = "RequestId to search. `requestId` or `startRequestDate` + `endRequestDate` or `startCreateDate` + `endCreateDate` must be filled")]
        [OpenApiParameter(name: "startRequestDate", Type = typeof(string), In = ParameterLocation.Query, Required = false, Description = "Message sending request start date (`yyyy-MM-dd HH:mm:ss`)")]
        [OpenApiParameter(name: "endRequestDate", Type = typeof(string), In = ParameterLocation.Query, Required = false, Description = "Message sending request end date (`yyyy-MM-dd HH:mm:ss`)")]
        [OpenApiParameter(name: "startCreateDate", Type = typeof(string), In = ParameterLocation.Query, Required = false, Description = "Message sending registration start date (`yyyy-MM-dd HH:mm:ss`)")]
        [OpenApiParameter(name: "endCreateDate", Type = typeof(string), In = ParameterLocation.Query, Required = false, Description = "Message sending registration end date (`yyyy-MM-dd HH:mm:ss`)")]
        [OpenApiParameter(name: "startResultDate", Type = typeof(string), In = ParameterLocation.Query, Required = false, Description = "Message sending complete start date (`yyyy-MM-dd HH:mm:ss`)")]
        [OpenApiParameter(name: "endResultDate", Type = typeof(string), In = ParameterLocation.Query, Required = false, Description = "Message sending complete end date (`yyyy-MM-dd HH:mm:ss`)")]
        [OpenApiParameter(name: "sendNo", Type = typeof(string), In = ParameterLocation.Query, Required = false, Description = "Sender's phone number")]
        [OpenApiParameter(name: "recipientNo", Type = typeof(string), In = ParameterLocation.Query, Required = false, Description = "Receiver's phone number")]
        [OpenApiParameter(name: "templateId", Type = typeof(string), In = ParameterLocation.Query, Required = false, Description = "Template number")]
        [OpenApiParameter(name: "msgStatus", Type = typeof(string), In = ParameterLocation.Query, Required = false, Description = "Message status code (`0`: fail, `1`: request, `2`: processing, `3`: success, `4`: Reservation cancellation, `5`: Duplicate failed)")]
        [OpenApiParameter(name: "resultCode", Type = typeof(string), In = ParameterLocation.Query, Required = false, Description = "Receive result code (`MTR1`: success, `MTR2`: fail)")]
        [OpenApiParameter(name: "subResultCode", Type = typeof(string), In = ParameterLocation.Query, Required = false, Description = "Receive result detail code (`MTR2_1`: Validation failed, `MTR2_2`: carrier problem, `MTR2_3`: Device problem)")]
        [OpenApiParameter(name: "senderGroupingKey", Type = typeof(string), In = ParameterLocation.Query, Required = false, Description = "Sender's group key")]
        [OpenApiParameter(name: "recipientGroupingKey", Type = typeof(string), In = ParameterLocation.Query, Required = false, Description = "Receiver's group key")]
        [OpenApiParameter(name: "pageNum", Type = typeof(string), In = ParameterLocation.Query, Required = false, Description = "Page number in the pagination. Default value is '1'")]
        [OpenApiParameter(name: "pageSize", Type = typeof(string), In = ParameterLocation.Query, Required = false, Description = "Page size in the pagination. Default value is '15'")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(object), Description = "The OK response")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "The input was invalid")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.InternalServerError, Description = "The service has got an unexpected error")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "messages")] HttpRequest req)
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

            var baseUrl = this._settings.BaseUrl;
            var version = this._settings.Version;
            var endpoint = this._settings.Endpoints.ListMessages;
            var options = new ListMessagesRequestUrlOptions()
            {
                Version = version,
                AppKey = headers.AppKey,
                RequestId = req.Query["requestId"].ToString(),
                StartRequestDate = req.Query["startRequestDate"].ToString(),
                EndRequestDate = req.Query["endRequestDate"].ToString(),
                StartCreateDate = req.Query["startCreateDate"].ToString(),
                EndCreateDate = req.Query["endCreateDate"].ToString(),
                StartResultDate = req.Query["startResultDate"].ToString(),
                EndResultDate = req.Query["endResultDate"].ToString(),
                SendNo = req.Query["sendNo"].ToString(),
                RecipientNo = req.Query["recipientNo"].ToString(),
                TemplateId = req.Query["templateId"].ToString(),
                MsgStatus = req.Query["msgStatus"].ToString(),
                ResultCode = req.Query["resultCode"].ToString(),
                SubResultCode = req.Query["subResultCode"].ToString(),
                SenderGroupingKey = req.Query["senderGroupingKey"].ToString(),
                RecipientGroupingKey = req.Query["recipientGroupingKey"].ToString(),
                PageNum = int.TryParse(req.Query["pageNum"].ToString(), out int pageNumParse) ? pageNumParse : 1,
                PageSize = int.TryParse(req.Query["pageSize"].ToString(), out int pageSizeParse) ? pageSizeParse : 15
            };
            var requestUrl = this._settings.Formatter.Format($"{baseUrl.TrimEnd('/')}/{endpoint.TrimStart('/')}", options);

            this._http.DefaultRequestHeaders.Add("X-Secret-Key", headers.SecretKey);
            var result = await this._http.GetAsync(requestUrl).ConfigureAwait(false);

            var payload = await result.Content.ReadAsAsync<object>().ConfigureAwait(false);

            return new OkObjectResult(payload);
        }
    }
}