using O24OpenAPI.Web.CMS.Models;

namespace O24OpenAPI.Web.CMS.Services.Interfaces;

public interface ICommonService
{
    Task<dynamic> GetPagedList(SearchPagedListModel model);
}
