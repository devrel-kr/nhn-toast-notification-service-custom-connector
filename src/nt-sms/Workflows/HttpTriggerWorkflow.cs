using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Aliencube.AzureFunctions.Extensions.Common;

using FluentValidation;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        Task<IHttpTriggerWorkflow> ValidateQueriesAsync<T>(HttpRequest req, IValidator<T> validator) where T : BaseRequestQueries;
    }

    /// <summary>
    /// This represents the workflow entity for the HTTP triggers.
    /// </summary>
    public class HttpTriggerWorkflow : IHttpTriggerWorkflow
    {
        private RequestHeaderModel _headers;
        private BaseRequestQueries _queries;

        /// <inheritdoc />
        public async Task<IHttpTriggerWorkflow> ValidateHeaderAsync(HttpRequest req)
        {
            var headers = req.To<RequestHeaderModel>(useBasicAuthHeader: true)
                             .Validate();

            this._headers = headers;

            return await Task.FromResult(this).ConfigureAwait(false);
        }

        public async Task<IHttpTriggerWorkflow> ValidateQueriesAsync<T>(HttpRequest req, IValidator<T> validator) where T : BaseRequestQueries
        {
            var queries = await req.To<T>(SourceFrom.Query)
                                   .Validate(validator)
                                   .ConfigureAwait(false);

            this._queries = queries;

            return await Task.FromResult(this).ConfigureAwait(false);
        }
    }
}