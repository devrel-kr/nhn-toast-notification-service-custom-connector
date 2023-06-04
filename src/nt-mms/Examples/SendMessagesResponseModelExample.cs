using System.Collections.Generic;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

using Toast.Common.Models;
using Toast.Mms.Models;

namespace Toast.Mms.Examples
{
    /// <summary>
    /// This represents the example entity for SendMessages response body.
    /// </summary>
    public class SendMessagesResponseModelExample : OpenApiExample<SendMessagesResponseBody>
    {
        public override IOpenApiExample<SendMessagesResponseBody> Build(NamingStrategy namingStrategy = null)
        {
            var exampleInstance = new SendMessagesResponseBody()
            {
                Header = new ResponseHeaderModel()
                {
                    IsSuccessful = true,
                    ResultCode = 0,
                    ResultMessage = "SUCCESS"
                },
                Body = new ResponseItemBodyModel<SendMessagesResponseData>()
                {
                    Data = new SendMessagesResponseData()
                    {
                        RequestId = "201808100000000000000000",
                        StatusCode = "2",
                        SenderGroupKey = "SenderGroupingKey",
                        SendResults = new List<SendMessagesResponseResult>()
                        {
                                    new SendMessagesResponseResult()
                                    {
                                        RecipientNumber = "01000000000",
                                        ResultCode = 0,
                                        ResultMessage = "SUCCESS",
                                        RecipientSequence = 1,
                                        RecipientGroupKey = "RecipientGroupingKey"
                                    }
                        }
                    }
                }
            };

            Examples.Add(
                OpenApiExampleResolver.Resolve(
                    "sample",
                    "This represents the example entity for SendMessages response body.",
                    exampleInstance,
                    namingStrategy
                )
            );

            return this;
        }
    }
}