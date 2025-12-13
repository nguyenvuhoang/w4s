using O24OpenAPI.Web.CMS.Services.Interfaces;
using O24OpenAPI.Web.CMS.Services.Interfaces.CBS;

namespace O24OpenAPI.Web.CMS.Services.O9Service;

public class O9CBService(IBaseService baseService) : ICBService
{
    private readonly IBaseService _baseService = baseService;

    public async Task<string> GetInfoTranDef(string tranCode)
    {
        try
        {
            string sql = @"SELECT TXTYPE FROM O9CBS.S_TXDEF WHERE TXCODE = '{0}'";
            sql = string.Format(sql, tranCode);
            var result = await _baseService.ExecuteSql<string>(sql);
            return result;
        }
        catch
        {
            return string.Empty;
        }
    }
}
