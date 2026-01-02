namespace O24OpenAPI.NCH.API.Application.Common;

public class GlobalVariable
{
    public static DateTimeOffset ExpireTime()
    {
        return DateTimeOffset.Now.AddDays(32400 / 24 / 60);
    }
}
