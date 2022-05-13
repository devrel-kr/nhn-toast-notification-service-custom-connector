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

using Toast.Common.Builders;
using Toast.Common.Configurations;
using Toast.Common.Models;
using Toast.Common.Validators;
using Toast.Sms.Configurations;
using Toast.Sms.Models;


namespace Toast.Sms.Triggers
{
    public class GetMessage
    {
        private readonly ToastSettings<SmsEndpointSettings> _settings;
        private readonly HttpClient _http;
        private readonly ILogger<GetMessage> _logger;

        public GetMessage(ToastSettings<SmsEndpointSettings> settings, IHttpClientFactory factory, ILogger<GetMessage> log)
        {
            this._settings = settings.ThrowIfNullOrDefault();
            this._http = factory.ThrowIfNullOrDefault().CreateClient("messages");
            this._logger = log.ThrowIfNullOrDefault();
        }

        [FunctionName(nameof(GetMessage))]
        [OpenApiOperation(operationId: "Messages.Get", tags: new[] { "messages" })]
        [OpenApiSecurity("app_key", SecuritySchemeType.ApiKey, Name = "x-app-key", In = OpenApiSecurityLocationType.Header, Description = "Toast app key")]
        [OpenApiSecurity("secret_key", SecuritySchemeType.ApiKey, Name = "x-secret-key", In = OpenApiSecurityLocationType.Header, Description = "Toast secret key")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header, Description = "Functions API key")]
        [OpenApiParameter(name: "requestId", Type = typeof(string), In = ParameterLocation.Path, Required = true, Description = "SMS request ID")]
        [OpenApiParameter(name: "recipientSeq", Type = typeof(string), In = ParameterLocation.Query, Required = true, Description = "SMS request sequence number")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(object), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "messages/{requestId:regex(^\\d+\\w+$)}")] HttpRequest req,
            string requestId)
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

            GetMessageRequestQueries queries = new GetMessageRequestQueries()
            {
                RecipientSequenceNumber = int.TryParse(req.Query["recipientSeq"].ToString(), out int recipientSeqVal) ? recipientSeqVal : 0
            };
            
            string[] paths = new string[] {"requestId", requestId };

            var requestUrl = new RequestUrlBuilder()
                .WithSettings<ToastSettings>(this._settings, this._settings.Endpoints.GetMessage)
                .WithHeaders(headers).WithQueries(queries)
                .WithPaths(paths).Build();
            
            var quries = new GetMessageRequestQueries();

            this._http.DefaultRequestHeaders.Add("X-Secret-Key", headers.SecretKey);
            var result = await this._http.GetAsync(requestUrl).ConfigureAwait(false);

            var payload = await result.Content.ReadAsAsync<object>().ConfigureAwait(false);

            return new OkObjectResult(payload);
        }
    }
}