using O24OpenAPI.CMS.API.Application.Models;
using O24OpenAPI.CMS.API.Application.Services.Interfaces;
using O24OpenAPI.Data.System.Linq;

namespace O24OpenAPI.CMS.API.Application.Services.Services;

public class CommonService(ITypeFinder typeFinder) : ICommonService
{
    private readonly ITypeFinder _typeFinder = typeFinder;

    public async Task<dynamic> GetPagedList(SearchPagedListModel model)
    {
        var entityType = _typeFinder.FindEntityTypeByName(model.EntityName);
        if (entityType != null)
        {
            var repositoryType = typeof(IRepository<>).MakeGenericType(entityType);
            var repository = EngineContext.Current.Resolve(repositoryType);

            var tableProperty = repositoryType.GetProperty("Table");
            var queryable = tableProperty.GetValue(repository) as IQueryable;

            var toPagedListMethod = typeof(AsyncIQueryableExtensions)
                .GetMethod("ToPagedList")
                .MakeGenericMethod(entityType);

            var result = await (dynamic)
                toPagedListMethod.Invoke(null, [queryable, model.PageIndex, model.PageSize, false]);
            return result;
        }
        return null;
    }
}
