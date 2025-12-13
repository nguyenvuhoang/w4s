namespace O24OpenAPI.Web.CMS.Commons;

public class NeptuneSession
{
    public static string SessionToken { get; private set; }

    public static void SetToken(string sessionToken)
    {
        SessionToken = sessionToken;
    }
}
