using O24OpenAPI.Core.Configuration;

namespace O24OpenAPI.Web.CMS.Utils;

public class ServiceUtils
{
    public static string GetServiceUrl(string serviceID)
    {
        return serviceID switch
        {
            "SMS" => Singleton<O24OpenAPIConfiguration>.Instance.URLSMS,
            _ => throw new NotImplementedException(),
        };
    }
}
