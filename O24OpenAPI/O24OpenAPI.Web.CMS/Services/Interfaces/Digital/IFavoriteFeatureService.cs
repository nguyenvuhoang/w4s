namespace O24OpenAPI.Web.CMS.Services.Interfaces.Digital;

public interface IFavoriteFeatureService
{
    Task<List<FavoriteFeature>> GetFavoriteFeatureByUserCode(string UserCode);

    Task<List<FavoriteFeature>> GetAll();

    Task<FavoriteFeature> GetById(int id);

    Task<FavoriteFeature> Insert(FavoriteFeature model);

    Task<FavoriteFeature> Update(FavoriteFeature model);

    Task<FavoriteFeature> DeleteById(int id);
}
