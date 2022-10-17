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

namespace Toast.Sms.Workflows
{
    /// <summary>
    /// This provides interface to the HTTP trigger workflows.
    /// </summary>
    public interface IHttpTriggerWorkflow
    {
        
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
        public async Task<T> InvokeAsync<T>(HttpMethod method) where T : ResponseModel
        {
            var request = new HttpRequestMessage(method, this._requestUrl);
            if (!this._payload.IsNullOrDefault())
            {
                request.Content = new ObjectContent(
                    this._payload.GetType(), this._payload, this._formatter);
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
        /// Invokes the API request.
        /// </summary>
        /// <typeparam name="T">Type of response model.</typeparam>
        /// <param name="workflow"><see cref="IHttpTriggerWorkflow"/> instance wrapped with <see cref="Task"/>.</param>
        /// <param name="method"><see cref="HttpMethod"/> value.</param>
        public static async Task<T> InvokeAsync<T>(this Task<IHttpTriggerWorkflow> workflow, HttpMethod method) where T : ResponseModel
        {
            var instance = await workflow.ConfigureAwait(false);

            return await instance.InvokeAsync<T>(method);
        }
    }
}