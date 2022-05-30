using System.Collections.Generic;

using Newtonsoft.Json;
using Toast.Common.Models;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Newtonsoft.Json.Serialization;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

namespace Toast.Sms.Models
{

    /// <summary>
    /// This represents the entity for ListMessages response.
    /// </summary>
    public class ListMessagesResponse : ResponseModel<ResponseCollectionBodyModel<ListMessagesResponseData>> { }

    /// <summary>
    /// This represents the entity for ListMessages response data.
    /// </summary>
    public class ListMessagesResponseData
    {
        /// <summary>
        /// Gets or sets the request ID.
        /// </summary>
        public virtual string RequestId { get; set; }

        /// <summary>
        /// Gets or sets the request date.
        /// </summary>
        public virtual string RequestDate { get; set; }

        /// <summary>
        /// Gets or sets the response date.
        /// </summary>
        [JsonProperty("resultDate")]
        public virtual string ResponseDate { get; set; }

        /// <summary>
        /// Gets or sets the template ID.
        /// </summary>
        public virtual string TemplateId { get; set; }

        /// <summary>
        /// Gets or sets the template name.
        /// </summary>
        public virtual string TemplateName { get; set; }

        /// <summary>
        /// Gets or sets the category ID.
        /// </summary>
        public virtual string CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the category name.
        /// </summary>
        public virtual string CategoryName { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        public virtual string Body { get; set; }

        /// <summary>
        /// Gets or sets the send number.
        /// </summary>
        [JsonProperty("sendNo")]
        public virtual string SenderNumber { get; set; }

        /// <summary>
        /// Gets or sets the country code.
        /// </summary>
        public virtual string CountryCode { get; set; }

        /// <summary>
        /// Gets or sets the recipient number.
        /// </summary>
        [JsonProperty("recipientNo")]
        public virtual string RecipientNumber { get; set; }

        /// <summary>
        /// Gets or sets the message status code.
        /// </summary>
        [JsonProperty("msgStatus")]
        public virtual string MessageStatus { get; set; }

        /// <summary>
        /// Gets or sets the message status code name.
        /// </summary>
        [JsonProperty("msgStatusName")]
        public virtual string MessageStatusName { get; set; }

        /// <summary>
        /// Gets or sets the result code.
        /// </summary>
        public virtual string ResultCode { get; set; }

        /// <summary>
        /// Gets or sets the result code name.
        /// </summary>
        public virtual string ResultCodeName { get; set; }

        /// <summary>
        /// Gets or sets the telecom code.
        /// </summary>
        [JsonProperty("telecomCode")]
        public virtual int CarrierCode { get; set; }

        /// <summary>
        /// Gets or sets the telecom name.
        /// </summary>
        [JsonProperty("telecomCodeName")]
        public virtual string CarrierName { get; set; }

        /// <summary>
        /// Gets or sets the recipient sequence.
        /// </summary>
        [JsonProperty("recipientSeq")]
        public virtual int RecipientSequence { get; set; }

        /// <summary>
        /// Gets or sets the send type.
        /// </summary>
        public virtual string SendType { get; set; }

        /// <summary>
        /// Gets or sets the message type.
        /// </summary>

        public virtual string MessageType { get; set; }

        /// <summary>
        /// Gets or sets the send request ID.
        /// </summary>
        public virtual string UserId { get; set; }

        /// <summary>
        /// Gets or sets the whether advertisement.
        /// </summary>
        [JsonProperty("adYn")]
        public virtual string IsAdvertisement { get; set; }

        /// <summary>
        /// Gets or sets the result message.
        /// </summary>
        public virtual string ResultMessage { get; set; }

        /// <summary>
        /// Gets or sets the sender group key.
        /// </summary>
        [JsonProperty("senderGroupingKey")]
        public virtual string SenderGroupKey { get; set; }

        /// <summary>
        /// Gets or sets the recipient group key.
        /// </summary>
        [JsonProperty("recipientGroupingKey")]
        public virtual string RecipientGroupKey { get; set; }
    }

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
            

            this.Examples.Add(
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