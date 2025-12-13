using O24OpenAPI.Web.CMS.Models;
using O24OpenAPI.Web.CMS.Models.Portal;

namespace O24OpenAPI.Web.CMS.Services.Services.Portal;

public interface IRoleProfileService
{
    Task<string> LoadMenuByChannel(ModelWithChannel model);
    Task<string> LoadRoleOperation();
    Task UpdateUserRight(UserRightUpdateModel model);
    Task UpdateUserInRole(UpdateUserInRoleModel model);
}
