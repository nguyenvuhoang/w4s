using Newtonsoft.Json.Linq;

namespace O24OpenAPI.Web.CMS.Services.Interfaces;

public interface IUserAccountService
{
    Task<JToken> GetStaffCareInfo(string userId);
    Task<JToken> GetStaffCareList();
    Task<JToken> GetUserAccountList();
}
