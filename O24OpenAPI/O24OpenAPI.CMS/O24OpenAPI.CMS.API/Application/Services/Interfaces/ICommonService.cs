using O24OpenAPI.CMS.API.Application.Models;

namespace O24OpenAPI.CMS.API.Application.Services.Interfaces;

public interface ICommonService
{
    Task<dynamic> GetPagedList(SearchPagedListModel model);
}
