using System.Collections.Generic;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

using Toast.Common.Models;
using Toast.Sms.Verification.Models;

namespace Toast.Sms.Verification.Examples
{
    /// <summary>
    /// This represents the example entity for ListSenders response body.
    /// </summary>
    public class ListSendersResponseModelExample : OpenApiExample<ListSendersResponse>
    {
        public override IOpenApiExample<ListSendersResponse> Build(NamingStrategy namingStrategy = null)
        {
            var exampleInstance = new ListSendersResponse()
            {
                Header = new ResponseHeaderModel()
                {
                    ResultCode = 0,
                    ResultMessage = "SUCCESS",
                    IsSuccessful = true
                },
                Body = new ResponseCollectionBodyModel<ListSendersResponseData>()
                {
                    PageNumber = 1,
                    PageSize = 15,
                    TotalCount = 2,
                    Data = new List<ListSendersResponseData>()
                    {
                        new ListSendersResponseData()
                        {
                            ServiceId = 1234,
                            SenderNumber = "01012345678",
                            UseNumber = "Y",
                            BlockedNumber = "N",
                            BlockedReason = null,
                            CreateDate = "2020-01-01 00:00:00",
                            CreateUser = "18ad9058-6466-48ef-8a78-08c27519ac24",
                            UpdateDate = "2020-01-01 00:00:00",
                            UpdateUser = "18ad9058-6466-48ef-8a78-08c27519ac24",
                        },
                        new ListSendersResponseData()
                        {
                            ServiceId = 5678,
                            SenderNumber = "01087654321",
                            UseNumber = "Y",
                            BlockedNumber = "N",
                            BlockedReason = null,
                            CreateDate = "2020-01-01 00:00:00",
                            CreateUser = "18ad9058-6466-48ef-8a78-08c27519ac24",
                            UpdateDate = "2020-01-01 00:00:00",
                            UpdateUser = "18ad9058-6466-48ef-8a78-08c27519ac24",
                        }
                    }
                }
            };

            Examples.Add(
                OpenApiExampleResolver.Resolve(
                    "sample",
                    "This represents the example entity for GetMessage response body.",
                    exampleInstance,
                    namingStrategy
            ));

            return this;
        }
    }
}