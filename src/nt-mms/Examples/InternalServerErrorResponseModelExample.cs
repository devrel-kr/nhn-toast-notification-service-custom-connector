using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

using Toast.Common.Models;

namespace Toast.Mms.Examples
{
    /// <summary>
    /// This represents the example entity for the Internal Server Error response body.
    /// </summary>
    public class InternalServerErrorResponseModelExample : OpenApiExample<ErrorResponseModel>
    {
        public override IOpenApiExample<ErrorResponseModel> Build(NamingStrategy namingStrategy = null)
        {
            var exampleInstance = new ErrorResponseModel()
            {
                Message = "Something has gone wrong."
            };

            Examples.Add(
                OpenApiExampleResolver.Resolve(
                    "sample",
                    "This represents the example entity for the Internal Server Error response body.",
                    exampleInstance,
                    namingStrategy
                )
            );

            return this;
        }
    }
}