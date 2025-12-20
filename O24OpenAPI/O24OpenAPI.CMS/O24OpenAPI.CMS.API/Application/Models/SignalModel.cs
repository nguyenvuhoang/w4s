namespace O24OpenAPI.CMS.API.Application.Models;

public class SignalSendToUserModel : BaseTransactionModel
{
    public string Channel { get; set; }
    public string UserCode { get; set; }
    public string DeviceId { get; set; }
    public object Message { get; set; }
}
