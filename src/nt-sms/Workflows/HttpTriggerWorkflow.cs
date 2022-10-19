using System;
using System.Net.Http;
using System.Net.Http.Formatting;
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

namespace Toast.Sms.Workflows{
    /// <summary>
    /// This provides interface to the HTTP trigger workflows.
    /// </summary>
    public interface IHttpTriggerWorkflow{
        /// <summary>
        /// Validates the request header.
        /// </summary>
        /// <param name="req"><see cref="HttpRequest"/> instance.</param>
        /// <returns>Returns the <see cref="IHttpTriggerWorkflow"/> instance.</returns>
        Task<IHttpTriggerWorkflow> ValidateHeaderAsync(HttpRequest req);

        /// <summary>
        /// Builds the request URL with given path parameters.
        /// </summary>
        /// <param name="endpoint">API endpoint.</param>
        /// <param name="settings"><see cref="ToastSettings"/> instance.</param>
        /// <param name="paths">Instance inheriting <see cref="BaseRequestPaths"/> class.</param>
        /// <returns>Returns the <see cref="IHttpTriggerWorkflow"/> instance.</returns>
        Task<IHttpTriggerWorkflow> BuildRequestUrlAsync(string endpoint, ToastSettings settings, BaseRequestPaths paths = null);

    }

    /// <inheritdoc />
    public class HttpTriggerWorkflow : IHttpTriggerWorkflow{
        private readonly HttpClient _http;
        private RequestHeaderModel _headers;
        private BaseRequestQueries _queries;
        private string _requestUrl;
        private readonly MediaTypeFormatter _formatter;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpTriggerWorkflow"/> class.
        /// </summary>
        /// <param name="factory"><see cref="IHttpClientFactory"/> instance.</param>
        public HttpTriggerWorkflow(IHttpClientFactory factory, MediaTypeFormatter formatter)
        {
            this._http = factory.ThrowIfNullOrDefault().CreateClient("messages");
            this._formatter = formatter.ThrowIfNullOrDefault();
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
        public async Task<IHttpTriggerWorkflow> BuildRequestUrlAsync(string endpoint, ToastSettings settings, BaseRequestPaths paths = null)
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
    }

     /// <summary>
    /// This represents the extension class for <see cref="IHttpTriggerWorkflow"/>.
    /// </summary>
    public static class HttpTriggerWorkflowExtensions
    {
       /// <summary>
        /// Builds the request URL with given path parameters.
        /// </summary>
        /// <typeparam name="T">Type of request path object.</typeparam>
        /// <param name="workflow"><see cref="IHttpTriggerWorkflow"/> instance wrapped with <see cref="Task"/>.</param>
        /// <param name="endpoint">API endpoint.</param>
        /// <param name="settings"><see cref="ToastSettings"/> instance.</param>
        /// <param name="paths">Instance inheriting <see cref="BaseRequestPaths"/> class.</param>
        /// <returns>Returns the <see cref="IHttpTriggerWorkflow"/> instance.</returns>
        public static async Task<IHttpTriggerWorkflow> BuildRequestUrlAsync(this Task<IHttpTriggerWorkflow> workflow, string endpoint, ToastSettings settings, BaseRequestPaths paths = null)
        {
            var instance = await workflow.ConfigureAwait(false);

            return await instance.BuildRequestUrlAsync(endpoint, settings, paths);
        }

    }
}