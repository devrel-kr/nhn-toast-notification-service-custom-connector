using Toast.Common.Models;

namespace Toast.Sms.Models
{
    /// <summary>
    /// This represents the entity for GetMessage response.
    /// </summary>
    public class GetMessageResponse : ResponseModel<GetMessageResponseBody>
    { }

    /// <summary>
    /// This represents the entity for GetMessage response body.
    /// </summary>
    public class GetMessageResponseBody : ResponseItemBodyModel<GetMessageResponseData> 
    { }
}