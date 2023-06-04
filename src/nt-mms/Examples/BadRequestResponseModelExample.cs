using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

using Toast.Common.Models;

namespace Toast.Mms.Examples
{
    /// <summary>
    /// This represents the example entity for the Bad Request response body.
    /// </summary>
    public class BadRequestResponseModelExample : OpenApiExample<ErrorResponseModel>
    {
        public override IOpenApiExample<ErrorResponseModel> Build(NamingStrategy namingStrategy = null)
        {
            var exampleInstance = new ErrorResponseModel()
            {
                Message = "Invalid request."
            };

            Examples.Add(
                OpenApiExampleResolver.Resolve(
                    "sample",
                    "This represents the example entity for the Bad Request response body.",
                    exampleInstance,
                    namingStrategy
                )
            );

            return this;
        }
    }
}