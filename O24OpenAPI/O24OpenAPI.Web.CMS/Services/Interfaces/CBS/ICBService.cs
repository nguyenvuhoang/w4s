namespace O24OpenAPI.Web.CMS.Services.Interfaces.CBS;

public interface ICBService
{
    Task<string> GetInfoTranDef(string tranCode);
}
