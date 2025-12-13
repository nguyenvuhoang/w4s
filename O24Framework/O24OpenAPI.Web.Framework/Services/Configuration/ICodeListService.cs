using O24OpenAPI.Core;
using O24OpenAPI.Web.Framework.Domain;
using O24OpenAPI.Web.Framework.Models;

namespace O24OpenAPI.Web.Framework.Services.Configuration;

public interface ICodeListService
{
    Task<IPagedList<C_CODELIST>> GetByGroupAndName(CodeListGroupAndNameRequestModel model);
}
