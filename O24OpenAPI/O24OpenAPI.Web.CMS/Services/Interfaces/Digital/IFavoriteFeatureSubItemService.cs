namespace O24OpenAPI.Web.CMS.Services.Interfaces.Digital;

public interface IFavoriteFeatureSubItemService
{

    Task<FavoriteFeatureSubItem> GetById(int id);

    Task<FavoriteFeatureSubItem> Insert(FavoriteFeatureSubItem model);

    Task<List<FavoriteFeatureSubItem>> GetAll();

    Task<FavoriteFeatureSubItem> Update(FavoriteFeatureSubItem model);

    Task<FavoriteFeatureSubItem> DeleteById(int id);


}
