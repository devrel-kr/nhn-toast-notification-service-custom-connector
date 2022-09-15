using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

using Aliencube.AzureFunctions.Extensions.Common;

using FluentValidation;

using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;

using Newtonsoft.Json;

using Toast.Common.Builders;
using Toast.Common.Configurations;
using Toast.Common.Extensions;
using Toast.Common.Models;
using Toast.Common.Validators;
using Toast.Sms.Validators;

namespace Toast.Sms.Workflows
{
    /// <summary>
    /// This provides interface to the HTTP trigger workflows.
    /// </summary>
    public interface IHttpTriggerWorkflow
    {
        /// <summary>
        /// Validates the request header.
        /// </summary>
        /// <param name="req"><see cref="HttpRequest"/> instance.</param>
        /// <returns>Returns the <see cref="IHttpTriggerWorkflow"/> instance.</returns>
        Task<IHttpTriggerWorkflow> ValidateHeaderAsync(HttpRequest req);

        /// <summary>
        /// Validates the request queries.
        /// </summary>
        /// <typeparam name="T">Type of the request query object.</typeparam>
        /// <param name="req"><see cref="HttpRequest"/> instance.</param>
        /// <param name="validator"><see cref="IValidator{T}"/> instance.</param>
        /// <returns>Returns the <see cref="IHttpTriggerWorkflow"/> instance.</returns>
        Task<IHttpTriggerWorkflow> ValidateQueriesAsync<T>(HttpRequest req, IValidator<T> validator) where T : BaseRequestQueries;

        /// <summary>
        /// Builds the request URL with given path parameters.
        /// </summary>
        /// <typeparam name="T">Type of request path object.</typeparam>
        /// <param name="endpoint">API endpoint.</param>
        /// <param name="settings"><see cref="ToastSettings"/> instance.</param>
        /// <param name="paths">Instance inheriting <see cref="BaseRequestPaths"/> class.</param>
        /// <returns>Returns the <see cref="IHttpTriggerWorkflow"/> instance.</returns>
        Task<IHttpTriggerWorkflow> BuildRequestUrlAsync<T>(string endpoint, ToastSettings settings, T paths = null) where T : BaseRequestPaths;

        /// <summary>
        /// Invokes the API request.
        /// </summary>
        /// <typeparam name="T">Type of response model.</typeparam>
        /// <param name="method"><see cref="HttpMethod"/> value.</param>
        /// <returns>Returns the instance inheriting <see cref="ResponseModel"/> class.</returns>
        Task<T> InvokeAsync<T>(HttpMethod method) where T : ResponseModel;
    }

    /// <summary>
    /// This represents the workflow entity for the HTTP triggers.
    /// </summary>
    public class HttpTriggerWorkflow : IHttpTriggerWorkflow
    {
        private readonly HttpClient _http;

        private RequestHeaderModel _headers;
        private BaseRequestQueries _queries;
        private BaseRequestPayload _payload;
        private string _requestUrl;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpTriggerWorkflow"/> class.
        /// </summary>
        /// <param name="factory"><see cref="IHttpClientFactory"/> instance.</param>
        public HttpTriggerWorkflow(IHttpClientFactory factory)
        {
            this._http = factory.ThrowIfNullOrDefault().CreateClient("messages");
        }

        /// <inheritdoc />
        public async Task<IHttpTriggerWorkflow> ValidateHeaderAsync(HttpRequest req)
        {
            var headers = req.To<RequestHeaderModel>(useBasicAuthHeader: true)
                             .Validate();

            this._headers = headers;

            return await Task.FromResult(this).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<IHttpTriggerWorkflow> ValidateQueriesAsync<T>(HttpRequest req, IValidator<T> validator) where T : BaseRequestQueries
        {
            var queries = await req.To<T>(SourceFrom.Query)
                                   .Validate(validator)
                                   .ConfigureAwait(false);

            this._queries = queries;

            return await Task.FromResult(this).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<IHttpTriggerWorkflow> BuildRequestUrlAsync<T>(string endpoint, ToastSettings settings, T paths = null) where T : BaseRequestPaths
        {
            var builder = new RequestUrlBuilder()
                             .WithSettings(settings, endpoint)
                             .WithHeaders(this._headers)
                             .WithQueries(this._queries);

            if (!paths.IsNullOrDefault())
            {
                builder = builder.WithPaths(paths);
            }

            var requestUrl = builder.Build();

            this._requestUrl = requestUrl;

            return await Task.FromResult(this).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<T> InvokeAsync<T>(HttpMethod method) where T : ResponseModel
        {
            var request = new HttpRequestMessage(method, this._requestUrl);
            if (!this._payload.IsNullOrDefault())
            {
                var serialised = JsonConvert.SerializeObject(this._payload);
                request.Content = new StringContent(serialised, Encoding.UTF8, MediaTypeNames.Application.Json);
            }

            this._http.DefaultRequestHeaders.Add("X-Secret-Key", this._headers.SecretKey);
            var result = await this._http.SendAsync(request).ConfigureAwait(false);

            var payload = await result.Content.ReadAsAsync<T>().ConfigureAwait(false);

            return await Task.FromResult(payload).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// This represents the extension class for <see cref="IHttpTriggerWorkflow"/>.
    /// </summary>
    public static class HttpTriggerWorkflowExtensions
    {
        /// <summary>
        /// Validates the request queries.
        /// </summary>
        /// <typeparam name="T">Type of the request query object.</typeparam>
        /// <param name="workflow"><see cref="IHttpTriggerWorkflow"/> instance wrapped with <see cref="Task"/>.</param>
        /// <param name="req"><see cref="HttpRequest"/> instance.</param>
        /// <param name="validator"><see cref="IValidator{T}"/> instance.</param>
        /// <returns>Returns the <see cref="IHttpTriggerWorkflow"/> instance.</returns>
        public static async Task<IHttpTriggerWorkflow> ValidateQueriesAsync<T>(this Task<IHttpTriggerWorkflow> workflow, HttpRequest req, IValidator<T> validator) where T : BaseRequestQueries
        {
            var instance = await workflow.ConfigureAwait(false);

            return await instance.ValidateQueriesAsync(req, validator);
        }

        /// <summary>
        /// Builds the request URL with given path parameters.
        /// </summary>
        /// <typeparam name="T">Type of request path object.</typeparam>
        /// <param name="workflow"><see cref="IHttpTriggerWorkflow"/> instance wrapped with <see cref="Task"/>.</param>
        /// <param name="endpoint">API endpoint.</param>
        /// <param name="settings"><see cref="ToastSettings"/> instance.</param>
        /// <param name="paths">Instance inheriting <see cref="BaseRequestPaths"/> class.</param>
        /// <returns>Returns the <see cref="IHttpTriggerWorkflow"/> instance.</returns>
        public static async Task<IHttpTriggerWorkflow> BuildRequestUrlAsync<T>(this Task<IHttpTriggerWorkflow> workflow, string endpoint, ToastSettings settings, T paths = null) where T : BaseRequestPaths
        {
            var instance = await workflow.ConfigureAwait(false);

            return await instance.BuildRequestUrlAsync<T>(endpoint, settings, paths);
        }

        /// <summary>
        /// Validates the request payload.
        /// </summary>
        /// <typeparam name="T">Type of the request payload object.</typeparam>
        /// <param name="workflow"><see cref="IHttpTriggerWorkflow"/> instance wrapped with <see cref="Task"/>.</param>
        /// <param name="method"><see cref="HttpMethod"/> value.</param>
        public static async Task<T> InvokeAsync<T>(this Task<IHttpTriggerWorkflow> workflow, HttpMethod method) where T : ResponseModel
        {
            var instance = await workflow.ConfigureAwait(false);

            return await instance.InvokeAsync<T>(method);
        }
    }
}