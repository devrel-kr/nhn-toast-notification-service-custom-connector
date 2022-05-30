using Toast.Common.Models;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Newtonsoft.Json.Serialization;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using System.Collections.Generic;

namespace Toast.Sms.Models
{
    /// <summary>
    /// This represents the entity for ListMessageStatus response.
    /// </summary>
    public class ListMessageStatusResponse : ResponseModel<ResponseCollectionBodyModel<ListMessageStatusResponseData>> { }

    /// <summary>
    /// This represents the entity for ListMessageStatus response data.
    /// </summary>
    public class ListMessageStatusResponseData
    {
        /// <summary>
        /// Gets or sets the message type name.
        /// </summary>
        public virtual string MessageType { get; set; }

        /// <summary>
        /// Gets or sets the request ID.
        /// </summary>
        public virtual string RequestId { get; set; }

        /// <summary>
        /// Gets or sets the request sequence number.
        /// </summary>
        [JsonProperty("recipientSeq")]
        public virtual int RecipientSequence { get; set; }

        /// <summary>
        /// Gets or sets the message's result code.
        /// </summary>
        public virtual string ResultCode { get; set; }

        /// <summary>
        /// Gets or sets the message's result name.
        /// </summary>
        [JsonProperty("resultCodeName")]
        public virtual string ResultName { get; set; }

        /// <summary>
        /// Gets or sets the date requested.
        /// </summary>
        public virtual string RequestDate { get; set; }

        /// <summary>
        /// Gets or sets the date resulted.
        /// </summary>
        public virtual string ResultDate { get; set; }

        /// <summary>
        /// Gets or sets the date updated.
        /// </summary>
        public virtual string UpdateDate { get; set; }

        /// <summary>
        /// Gets or sets the sender's telecom code.
        /// </summary>
        [JsonProperty("telecomCode")]
        public virtual string CarrierCode { get; set; }

        /// <summary>
        /// Gets or sets the sender's telecom name.
        /// </summary>
        [JsonProperty("telecomCodeName")]
        public virtual string CarrierName { get; set; }

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
            this.Examples.Add(
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