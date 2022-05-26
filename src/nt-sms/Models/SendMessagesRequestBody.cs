using System.Collections.Generic;

using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Newtonsoft.Json.Serialization;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

namespace Toast.Sms.Models
{
    /// <summary>
    /// This represents the entity for SendMessages request body.
    /// </summary>
    public class SendMessagesRequestBody
    {
        /// <summary>
        /// Gets or sets the template ID.
        /// </summary>
        public virtual string TemplateId { get; set; }
        
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
        /// Gets or sets the date requested.
        /// </summary>
        public virtual string RequestDate { get; set; }

        /// <summary>
        /// Gets or sets the sender's grouping key.
        /// </summary>
        public virtual string SenderGroupingKey { get; set; }

        /// <summary>
        /// Gets or sets the list of recipients.
        /// </summary>
        [JsonProperty("recipientList")]
        public virtual List<SendMessagesRequestRecipient> Recipients { get; set; }

        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        public virtual string UserId { get; set; }

        /// <summary>
        /// Gets or sets the stats ID.
        /// </summary>
        public virtual string StatsId { get; set; }
    }

    /// <summary>
    /// This represents the example entity for SendMessages request body.
    /// </summary>
    public class SendMessagesRequestBodyModelExample : OpenApiExample<SendMessagesRequestBody>
    {
        public override IOpenApiExample<SendMessagesRequestBody> Build(NamingStrategy namingStrategy = null)
        {

            var exampleInstance = new SendMessagesRequestBody()
            {
                TemplateId = "TemplateId",
                Body = "body",
                SenderNumber = "00000000",
                RequestDate = "2018-08-10 10:00",
                SenderGroupingKey = "SenderGroupingKey",
                Recipients = new List<SendMessagesRequestRecipient>()
                {
                        new SendMessagesRequestRecipient()
                        {
                            RecipientNumber = "01000000000",
                            CountryCode = "82",
                            InternationalRecipientNumber = "821000000000",
                            TemplateParameters = new Dictionary<string, object>()
                            {
                                { "key", "value"}
                            },
                            RecipientGroupingKey = "recipientGroupingKey"
                        }
                },
                UserId = "UserId",
                StatsId = "statsId"
            };

            this.Examples.Add(
            OpenApiExampleResolver.Resolve(
                "sample",
                "This represents the example entity for SendMessages request body.",
                exampleInstance,
                namingStrategy
            ));

            return this;
        }
    }
}