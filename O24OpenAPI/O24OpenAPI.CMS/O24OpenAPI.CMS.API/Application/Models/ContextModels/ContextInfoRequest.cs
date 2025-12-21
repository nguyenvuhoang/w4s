using O24OpenAPI.CMS.API.Application.Features.Requests;

namespace O24OpenAPI.CMS.API.Application.Models.ContextModels;

public class ContextInfoRequestModel
{
    public ContextInfoRequestModel() { }

    public string ExecuteId { get; set; } = Guid.NewGuid().ToString();
    public int RequestId { get; set; }
    private string Ip = "";
    private string ClientOs = "";
    private string ClientBrowser = "";
    public string Language { get; set; } = "";
    private BoRequestModel RequestJson = new();
    private RequestModel RequestModel = new RequestModel();

    public string GetIp() => Ip;

    public void SetIp(string ip)
    {
        Ip = ip;
    }

    public string GetClientOs() => ClientOs;

    public void SetClientOs(string clientOs)
    {
        ClientOs = clientOs;
    }

    public string GetClientBrowser() => ClientBrowser;

    public void SetClientBrowser(string clientBrowser)
    {
        ClientBrowser = clientBrowser;
    }

    public BoRequestModel GetRequestJson() => RequestJson;

    public void SetRequestJson(BoRequestModel requestJson)
    {
        RequestJson = requestJson;
    }

    public RequestModel GetRequestModel() => RequestModel;

    public void SetRequestModel(RequestModel requestModel)
    {
        RequestModel = requestModel;
    }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string DeviceID { get; set; } = string.Empty;

    /// <summary>
    /// DeviceType
    /// </summary>
    /// <value></value>
    public string DeviceType { get; set; } = string.Empty;

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string PortalToken { get; set; } = string.Empty;

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public DateTimeOffset SessionExpired { get; set; }

    public string OsVersion { get; set; } = string.Empty;
    public string AppVersion { get; set; } = string.Empty;
    public string DeviceName { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public bool IsEmulator { get; set; } = false;
    public bool IsRootedOrJailbroken { get; set; } = false;
    public string UserAgent { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
    public string Modelname { get; set; } = string.Empty;
    public string Network { get; set; } = string.Empty;
    public string Memory { get; set; } = string.Empty;
}
