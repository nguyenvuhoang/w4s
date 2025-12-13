namespace O24OpenAPI.Web.CMS.Services.Interfaces.Digital;

public interface IUserPortalService
{

    Task<S_USERPORTAL> GetById(int id);


    Task<List<S_USERPORTAL>> GetAll();


    Task<S_USERPORTAL> Update(S_USERPORTAL model);

    Task<S_USERPORTAL> DeleteById(int id);


}
