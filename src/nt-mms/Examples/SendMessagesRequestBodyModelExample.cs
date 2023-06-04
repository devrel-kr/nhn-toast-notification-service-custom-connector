using System.Collections.Generic;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

using Toast.Mms.Models;

namespace Toast.Mms.Examples
{
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
                Title = "Title",
                Body = "Body",
                SenderNumber = "15446859",
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
                StatsId = "statsId",
                OriginCode = "123456789"
            };

            Examples.Add(
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