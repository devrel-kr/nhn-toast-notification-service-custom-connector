using System.Collections.Generic;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

using Toast.Common.Models;
using Toast.Sms.Models;

namespace Toast.Sms.Examples
{
    /// <summary>
    /// This represents the example entity for ListMessages response body.
    /// </summary>
    public class ListMessagesResponseModelExample : OpenApiExample<ListMessagesResponse>
    {
        public override IOpenApiExample<ListMessagesResponse> Build(NamingStrategy namingStrategy = null)
        {
            var exampleInstance = new ListMessagesResponse()
            {
                Header = new ResponseHeaderModel()
                {
                    ResultCode = 0,
                    ResultMessage = "SUCCESS",
                    IsSuccessful = true
                },
                Body = new ResponseCollectionBodyModel<ListMessagesResponseData>()
                {
                    PageNumber = 1,
                    PageSize = 15,
                    TotalCount = 1,
                    Data = new List<ListMessagesResponseData>()
                            {
                                new ListMessagesResponseData()
                                {
                                    RequestId = "2018081000000000000000000",
                                    RequestDate = "2018-08-10 10:06:30.0",
                                    ResponseDate = "2018-08-10 10:06:42.0",
                                    TemplateId = "TemplateId",
                                    TemplateName = "템플릿명",
                                    CategoryId =  "0",
                                    CategoryName = "카테고리명",
                                    Body = "단문 테스트",
                                    SenderNumber = "00000000",
                                    CountryCode = "82",
                                    RecipientNumber = "01000000000",
                                    MessageStatus = "3",
                                    MessageStatusName = "성공",
                                    ResultCode = "1000",
                                    ResultCodeName = "성공",
                                    CarrierCode = 10001,
                                    CarrierName = "통신사",
                                    RecipientSequence = 1,
                                    SendType = "0",
                                    MessageType = "SMS",
                                    UserId = "tester",
                                    IsAdvertisement = "N",
                                    ResultMessage =  "",
                                    SenderGroupKey = "SenderGroupingKey",
                                    RecipientGroupKey = "RecipientGroupingKey"
                                }
                            }
                }
            };

            Examples.Add(
                OpenApiExampleResolver.Resolve(
                    "sample",
                    "This represents the example entity for ListMessages response body.",
                    exampleInstance,
                    namingStrategy
            ));

            return this;
        }
    }
}