using Toast.Common.Models;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Newtonsoft.Json.Serialization;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

namespace Toast.Sms.Models
{
    /// <summary>
    /// This represents the entity for GetMessage response.
    /// </summary>
    public class GetMessageResponse : ResponseModel<ResponseItemBodyModel<GetMessageResponseData>> { }

    /// <summary>
    /// This represents the entity for GetMessage response data.
    /// </summary>
    public class GetMessageResponseData
    {
        /// <summary>
        /// Gets or sets the request ID.
        /// </summary>
        public virtual string RequestId { get; set; }

        /// <summary>
        /// Gets or sets the date requested.
        /// </summary>
        public virtual string RequestDate { get; set; }

        /// <summary>
        /// Gets or sets the date resulted.
        /// </summary>
        public virtual string ResultDate { get; set; }

        /// <summary>
        /// Gets or sets the template ID.
        /// </summary>
        public virtual string TemplateId { get; set; }

        /// <summary>
        /// Gets or sets the template ID.
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
        /// Gets or sets the message body.
        /// </summary>
        public virtual string Body { get; set; }

        /// <summary>
        /// Gets or sets the sender's phone number.
        /// </summary>
        [JsonProperty("sendNo")]
        public virtual string SenderNumber { get; set; }

        /// <summary>
        /// Gets or sets the country code.
        /// </summary>
        public virtual string CountryCode { get; set; }

        /// <summary>
        /// Gets or sets the recipient's phone number.
        /// </summary>
        [JsonProperty("recipientNo")]
        public virtual string RecipientNumber { get; set; }

        /// <summary>
        /// Gets or sets the message's status code.
        /// </summary>
        [JsonProperty("msgStatus")]
        public virtual string MessageStatus { get; set; }

        /// <summary>
        /// Gets or sets the message's status name.
        /// </summary>
        [JsonProperty("msgStatusName")]
        public virtual string MessageStatusName { get; set; }

        /// <summary>
        /// Gets or sets the message's result code.
        /// </summary>
        public virtual string ResultCode { get; set; }

        /// <summary>
        /// Gets or sets the message's result name.
        /// </summary>
        public virtual string ResultCodeName { get; set; }

        /// <summary>
        /// Gets or sets the sender's telecom code.
        /// </summary>
        [JsonProperty("telecomCode")]
        public virtual int CarrierCode { get; set; }

        /// <summary>
        /// Gets or sets the sender's telecom name.
        /// </summary>
        [JsonProperty("telecomCodeName")]
        public virtual string CarrierName { get; set; }

        /// <summary>
        /// Gets or sets the request sequence number.
        /// </summary>
        [JsonProperty("recipientSeq")]
        public virtual int RecipientSequence { get; set; }

        /// <summary>
        /// Gets or sets the message type code.
        /// </summary>
        public virtual string SendType { get; set; }

        /// <summary>
        /// Gets or sets the message type name.
        /// </summary>
        public virtual string MessageType { get; set; }

        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        public virtual string UserId { get; set; }

        /// <summary>
        /// Gets or sets whether to advertise.
        /// </summary>
        [JsonProperty("adYn")]
        public virtual string IsAdvertisement { get; set; }

        /// <summary>
        /// Gets or sets the result message.
        /// </summary>
        public virtual string ResultMessage { get; set; }

        /// <summary>
        /// Gets or sets the sender's grouping key.
        /// </summary>
        [JsonProperty("senderGroupingKey")]
        public virtual string SenderGroupKey { get; set; }

        /// <summary>
        /// Gets or sets the recipient's grouping key.
        /// </summary>
        [JsonProperty("recipientGroupingKey")]
        public virtual string RecipientGroupKey { get; set; }
    }

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
            this.Examples.Add(
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