using O24OpenAPI.Web.CMS.Models.ContextModels;

namespace O24OpenAPI.Web.CMS.Utils;

public class SessionUtils
{
    private static AsyncLocal<UserSessions> _userSession = new();

    public static UserSessions GetUserSession(JWebUIObjectContextModel context = null)
    {
        context ??= EngineContext.Current.Resolve<JWebUIObjectContextModel>();
        return _userSession.Value ??= context.InfoUser.UserSession;
    }

    public static string GetUserCode(JWebUIObjectContextModel context = null)
    {
        return GetUserSession().UserCode;
    }
}
