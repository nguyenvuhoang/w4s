namespace O24OpenAPI.Web.CMS.Services.Factory;

public class EngineFactory
{
    public static T Resolve<T>(IServiceScope scope = null)
        where T : class
    {
        var _coreServiceFactory = EngineContext.Current.Resolve<ICoreServiceFactory>(scope);
        return _coreServiceFactory.CreateService<T>();
    }
}
