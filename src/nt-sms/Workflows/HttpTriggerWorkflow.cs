using System;
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
        /// Validates the request queries.
        /// </summary>
        /// <typeparam name="T">Type of the request query object.</typeparam>
        /// <param name="req"><see cref="HttpRequest"/> instance.</param>
        /// <param name="validator"><see cref="IValidator{T}"/> instance.</param>
        /// <returns>Returns the <see cref="IHttpTriggerWorkflow"/> instance.</returns>
        Task<IHttpTriggerWorkflow> ValidateQueriesAsync<T>(HttpRequest req, IValidator<T> validator) where T : BaseRequestQueries;
    }

    /// <summary>
    /// This represents the workflow entity for the HTTP triggers.
    /// </summary>
    public class HttpTriggerWorkflow : IHttpTriggerWorkflow
    {
        private readonly HttpClient _http;

        private BaseRequestQueries _queries;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpTriggerWorkflow"/> class.
        /// </summary>
        /// <param name="factory"><see cref="IHttpClientFactory"/> instance.</param>
        public HttpTriggerWorkflow(IHttpClientFactory factory)
        {
            this._http = factory.ThrowIfNullOrDefault().CreateClient("messages");
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
    }
}