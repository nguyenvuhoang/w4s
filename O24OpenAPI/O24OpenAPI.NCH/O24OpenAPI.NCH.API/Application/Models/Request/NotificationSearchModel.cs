using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.NCH.Models.Request;

public class NotificationSearchModel : BaseTransactionModel
{
    public NotificationSearchModel()
    {
        PageIndex = 0;
        PageSize = int.MaxValue;
    }

    public string UserCode { get; set; }
    public string AppType { get; set; }
    public string NotificationType { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}
