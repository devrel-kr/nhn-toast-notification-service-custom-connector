using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

using Toast.Common.Models;
using Toast.Sms.Models;

namespace Toast.Sms.Examples
{
    /// <summary>
    /// This represents the example entity for GetMessage response body.
    /// </summary>
    public class GetMessageResponseModelExample : OpenApiExample<GetMessageResponse>
    {
        public override IOpenApiExample<GetMessageResponse> Build(NamingStrategy namingStrategy = null)
        {
            var exampleInstance = new GetMessageResponse()
            {
                Header = new ResponseHeaderModel()
                {
                    ResultCode = 0,
                    ResultMessage = "SUCCESS",
                    IsSuccessful = true
                },
                Body = new ResponseItemBodyModel<GetMessageResponseData>()
                {
                    Data = new GetMessageResponseData()
                    {
                        RequestId = "2018081000000000000000000",
                        RequestDate = "2018-08-10 10:06:30.0",
                        ResultDate = "2018-08-10 10:06:42.0",
                        TemplateId = "TemplateId",
                        TemplateName = "TemplateName",
                        CategoryId = "0",
                        CategoryName = "CategoryName",
                        Body = "Body",
                        SenderNumber = "00000000",
                        CountryCode = "82",
                        RecipientNumber = "01000000000",
                        MessageStatus = "3",
                        MessageStatusName = "success",
                        ResultCode = "1000",
                        ResultCodeName = "success",
                        CarrierCode = 10001,
                        CarrierName = "SKT",
                        RecipientSequence = 1,
                        SendType = "0",
                        MessageType = "SMS",
                        UserId = "tester",
                        IsAdvertisement = "N",
                        ResultMessage = "success",
                        SenderGroupKey = "SenderGroupingKey",
                        RecipientGroupKey = "RecipientGroupingKey"
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