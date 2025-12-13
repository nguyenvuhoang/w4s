using Microsoft.EntityFrameworkCore;
using O24OpenAPI.Web.CMS.Services.Interfaces.Digital;
using O24OpenAPI.Web.Framework.Localization;

namespace O24OpenAPI.Web.CMS.Services.Services.Digital;

public class UserFavoriteFeatureService : IUserFavoriteFeatureService
{
    private readonly ILocalizationService _localizationService;

    private readonly IRepository<UserFavoriteFeature> _userFavoriteFeatureRepository;

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="localizationService"></param>
    /// <param name="currencyRepository"></param>
    public UserFavoriteFeatureService(
        ILocalizationService localizationService,
        IRepository<UserFavoriteFeature> userFavoriteFeatureRepository
    )
    {
        _localizationService = localizationService;
        _userFavoriteFeatureRepository = userFavoriteFeatureRepository;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<UserFavoriteFeature> DeleteById(int id)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public async Task<List<UserFavoriteFeature>> GetAll()
    {
        return await _userFavoriteFeatureRepository.Table.ToListAsync();
    }

    public async Task<UserFavoriteFeature> GetById(int id)
    {
        return await _userFavoriteFeatureRepository.GetById(id);
    }

    public async Task<UserFavoriteFeature> Insert(UserFavoriteFeature model)
    {
        var checkdata = _userFavoriteFeatureRepository
            .Table.Where(item =>
                item.UserCode == model.UserCode
                && item.FavoriteFeatureID == model.FavoriteFeatureID
            )
            .FirstOrDefaultAsync();
        if (checkdata != null)
        {
            await _userFavoriteFeatureRepository.Insert(model);
        }
        else
        {
            throw new O24OpenAPIException(
                "Common.Value.Unique",
                "The UserFavoriteFeature already existing in system"
            );
        }
        return model;
    }

    public Task<UserFavoriteFeature> Update(UserFavoriteFeature learnApi)
    {
        throw new NotImplementedException();
    }

    public async Task<List<UserFavoriteFeature>> InsertListUserFavoriteFeature(
        List<UserFavoriteFeature> listmodel
    )
    {
        await _userFavoriteFeatureRepository.BulkInsert(listmodel);
        return listmodel;
    }

    public async Task<List<UserFavoriteFeature>> InserListtUserFavoriteFeature(
        List<UserFavoriteFeature> listmodel
    )
    {
        await _userFavoriteFeatureRepository.BulkInsert(listmodel);
        return listmodel;
    }
}
