namespace O24OpenAPI.Web.CMS.Services.Interfaces.Digital;

public interface IUserFavoriteFeatureService
{

    Task<UserFavoriteFeature> GetById(int id);

    Task<UserFavoriteFeature> Insert(UserFavoriteFeature model);

    Task<List<UserFavoriteFeature>> InsertListUserFavoriteFeature(List<UserFavoriteFeature> listmodel);

    Task<List<UserFavoriteFeature>> GetAll();


    Task<UserFavoriteFeature> Update(UserFavoriteFeature model);

    Task<UserFavoriteFeature> DeleteById(int id);


}
