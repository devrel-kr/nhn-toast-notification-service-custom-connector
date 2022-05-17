using System.Collections.Generic;

using Newtonsoft.Json;
using Toast.Common.Models;

namespace Toast.Sms.Models
{
    /// <summary>
    /// This represents the entity for SendMessages response.
    /// </summary>
    public class SendMessagesResponse : ResponseModel<SendMessagesResponseBody>
    { }

    /// <summary>
    /// This represents the entity for SendMessages response body.
    /// </summary>
    public class SendMessagesResponseBody: ResponseItemBodyModel<SendMessagesResponseData>
    { }
        
}