namespace O24OpenAPI.Web.CMS.Services.Factory;

public interface ICoreServiceFactory
{
    T CreateService<T>()
        where T : class;
}
