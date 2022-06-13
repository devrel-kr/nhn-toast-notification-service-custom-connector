using System.Collections.Generic;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using Toast.Common.Models;

namespace Toast.Sms.Models
{
    /// <summary>
    /// This represents the entity for ListSenders response.
    /// </summary>
    public class ListSendersResponse : ResponseModel<ResponseCollectionBodyModel<ListSendersResponseData>> { }

    /// <summary>
    /// This represents the entity for ListSenders response data.
    /// </summary>
    public class ListSendersResponseData
    {
        /// <summary>
        /// Gets or sets the service ID.
        /// </summary>
        public virtual int ServiceId { get; set; }

        /// <summary>
        /// Gets or sets the sender's phone number.
        /// </summary>
        [JsonProperty("sendNo")]
        public virtual string SenderNumber { get; set; }

        /// <summary>
        /// Gets or sets the value indicating whether to use the sender's number or not.
        /// </summary>
        [JsonProperty("useYn")]
        public virtual string UseNumber { get; set; }

        /// <summary>
        /// Gets or sets the value indicating whether the sender's number is blocked or not.
        /// </summary>
        [JsonProperty("blockYn")]
        public virtual string BlockedNumber { get; set; }

        /// <summary>
        /// Gets or sets the reason the number is blocked.
        /// </summary>
        [JsonProperty("blockReason")]
        public virtual string BlockedReason { get; set; }

        /// <summary>
        /// Gets or sets the date created.
        /// </summary>
        public virtual string CreateDate { get; set;}

        /// <summary>
        /// Gets or sets the user ID who created the record.
        /// </summary>
        public virtual string CreateUser { get; set; }

        /// <summary>
        /// Gets or sets the date updated.
        /// </summary>
        public virtual string UpdateDate { get; set;}

        /// <summary>
        /// Gets or sets the user ID who updated the record.
        /// </summary>
        public virtual string UpdateUser { get; set; }
    }

    /// <summary>
    /// This represents the example entity for GetMessage response body.
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