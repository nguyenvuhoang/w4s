using O24OpenAPI.CMS.API.Application.Models;
using O24OpenAPI.CMS.API.Application.Models.Portal;

namespace O24OpenAPI.CMS.API.Application.Services.Services.Portal;

public interface IRoleProfileService
{
    Task<string> LoadMenuByChannel(ModelWithChannel model);
    Task<string> LoadRoleOperation();
    Task UpdateUserRight(UserRightUpdateModel model);
    Task UpdateUserInRole(UpdateUserInRoleModel model);
}
