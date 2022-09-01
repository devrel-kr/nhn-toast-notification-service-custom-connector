using System.Collections.Generic;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

using Toast.Common.Models;
using Toast.Sms.Models;

namespace Toast.Sms.Examples
{
    /// <summary>
    /// This represents the example entity for ListMessageStatus response body.
    /// </summary>
    public class ListMessageStatusResponseModelExample : OpenApiExample<ListMessageStatusResponse>
    {
        public override IOpenApiExample<ListMessageStatusResponse> Build(NamingStrategy namingStrategy = null)
        {
            var exampleInstance = new ListMessageStatusResponse()
            {
                Header = new ResponseHeaderModel()
                {
                    ResultCode = 0,
                    ResultMessage = "SUCCESS",
                    IsSuccessful = true
                },
                Body = new ResponseCollectionBodyModel<ListMessageStatusResponseData>()
                {
                    PageNumber = 1,
                    PageSize = 15,
                    TotalCount = 1,
                    Data = new List<ListMessageStatusResponseData>()
                            {
                                new ListMessageStatusResponseData()
                                {
                                    MessageType = "SMS",
                                    RequestId = "2018081000000000000000000",
                                    RecipientSequence = 1,
                                    ResultCode = "1000",
                                    ResultName = "success",
                                    RequestDate = "2018-08-10 10:06:30.0",
                                    ResultDate = "2018-08-10 10:06:42.0",
                                    UpdateDate = "2018-10-04 16:17:15.0",
                                    CarrierCode = "10003",
                                    CarrierName = "LGU",
                                    SenderGroupKey = "SenderGroupingKey",
                                    RecipientGroupKey = "RecipientGroupingKey"
                                }
                            }
                }
            };
            Examples.Add(
                OpenApiExampleResolver.Resolve(
                    "sample",
                    "This represents the example entity for ListMessageStatus response body.",
                    exampleInstance,
                    namingStrategy
            ));
            return this;
        }
    }
}