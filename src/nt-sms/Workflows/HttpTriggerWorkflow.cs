using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Aliencube.AzureFunctions.Extensions.Common;

using FluentValidation;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Toast.Common.Builders;
using Toast.Common.Configurations;
using Toast.Common.Extensions;
using Toast.Common.Models;
using Toast.Common.Validators;
using Toast.Sms.Configurations;
using Toast.Sms.Validators;
using System.Reflection;

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
    
        Task<IHttpTriggerWorkflow> BuildRequestUrl<Tresult>(ToastSettings<SmsEndpointSettings> settings, BaseRequestPaths paths = null);

        Task<IHttpTriggerWorkflow> InvokeAsync<T>();

    }

    /// <summary>
    /// This represents the workflow entity for the HTTP triggers.
    /// </summary>
    public class HttpTriggerWorkflow : IHttpTriggerWorkflow
    {
        private RequestHeaderModel _headers;
        private BaseRequestQueries _queries;
        private readonly HttpClient _http;
        private object _payload;
        private string _requestUrl;

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

        public async Task<IHttpTriggerWorkflow> BuildRequestUrl<Tresult>(ToastSettings<SmsEndpointSettings> settings, BaseRequestPaths paths = null) {
            // var paths = new GetMessageRequestPaths() { RequestId = requestId };

            Type type = settings.Endpoints.GetType();
            string name = nameof(Tresult);
            PropertyInfo endpoint = type.GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
 
            var requestUrl = new RequestUrlBuilder()
                .WithSettings(settings, endpoint.GetValue(settings.Endpoints).ToString())
                .WithHeaders(_headers) 
                .WithQueries(_queries)
                .WithPaths(paths).Build();

            this._requestUrl = requestUrl;
 
            return await Task.FromResult(this).ConfigureAwait(false);   
        }

        public async Task<IHttpTriggerWorkflow> InvokeAsync<T>()
        {
            this._http.DefaultRequestHeaders.Add("X-Secret-Key", _headers.SecretKey);
            var result = await this._http.GetAsync(_requestUrl).ConfigureAwait(false);
            
            var payload = await result.Content.ReadAsAsync<T>().ConfigureAwait(false);
            this._payload = payload;
                
            return await Task.FromResult(this).ConfigureAwait(false);
            
        }

    }
    public static class HttpTriggerWorkflowExtensions
    {
        public static async Task<IHttpTriggerWorkflow> ValidateQueriesAsync<T>(this Task<IHttpTriggerWorkflow> workflow, HttpRequest req, IValidator<T> validator) where T : BaseRequestQueries
        {
            var instance = await workflow.ConfigureAwait(false);

            return await instance.ValidateQueriesAsync(req, validator);
        }

        public static async Task<IHttpTriggerWorkflow> BuildRequestUrl<Tresult>(this Task<IHttpTriggerWorkflow> workflow, ToastSettings<SmsEndpointSettings> settings, BaseRequestPaths paths = null)
        {
            var instance = await workflow.ConfigureAwait(false);

            return await instance.BuildRequestUrl<Tresult>(settings, paths);
        }

        public static async Task<IHttpTriggerWorkflow> InvokeAsync<T>(this Task<IHttpTriggerWorkflow> workflow)
        {
            var instance = await workflow.ConfigureAwait(false);

            return await instance.InvokeAsync<T>();
        }
    }        

}