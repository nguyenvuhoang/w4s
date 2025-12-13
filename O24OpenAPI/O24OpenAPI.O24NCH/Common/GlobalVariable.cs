namespace O24OpenAPI.O24NCH.Common;

public class GlobalVariable
{
    public static DateTimeOffset ExpireTime()
    {
        return DateTimeOffset.Now.AddDays(32400 / 24 / 60);
    }
}
