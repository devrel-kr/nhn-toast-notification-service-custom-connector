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

    }

    /// <inheritdoc />
    public class HttpTriggerWorkflow : IHttpTriggerWorkflow{
        private readonly HttpClient _http;
        private RequestHeaderModel _headers;
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
    }

     /// <summary>
    /// This represents the extension class for <see cref="IHttpTriggerWorkflow"/>.
    /// </summary>
    public static class HttpTriggerWorkflowExtensions
    {
       
    }
}