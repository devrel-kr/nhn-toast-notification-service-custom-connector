using System.Collections.Generic;

using Newtonsoft.Json;
using Toast.Common.Models;

namespace Toast.Sms.Models
{

    /// <summary>
    /// This represents the entity for ListMessages response.
    /// </summary>
    public class ListMessagesResponse : ResponseModel<ListMessagesResponseBody>
    { }


    /// <summary>
    /// This represents the entity for ListMessages response body.
    /// </summary>
    public class ListMessagesResponseBody : ResponseCollectionBodyModel<ListMessagesResponseData> 
    { }
}