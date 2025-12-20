using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.Framework.Localization;
using O24OpenAPI.Web.CMS.Services.Interfaces.Digital;

namespace O24OpenAPI.Web.CMS.Services.Services.Digital;

public class FavoriteFeatureService : IFavoriteFeatureService
{
    private readonly ILocalizationService _localizationService;
    private readonly IRepository<FavoriteFeature> _favoriteFeatureRepository;

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="localizationService"></param>
    /// <param name="favoriteFeatureRepo"></param>
    public FavoriteFeatureService(
        ILocalizationService localizationService,
        IRepository<FavoriteFeature> favoriteFeatureRepo
    )
    {
        _localizationService = localizationService;
        _favoriteFeatureRepository = favoriteFeatureRepo;
    }

    public async Task<List<FavoriteFeature>> GetAll()
    {
        return await _favoriteFeatureRepository.Table.ToListAsync();
    }

    public Task<List<FavoriteFeature>> GetFavoriteFeatureByUserCode(string UserCode)
    {
        var result = _favoriteFeatureRepository
            .Table.Where(item => item.FavoriteFeatureCode == UserCode)
            .ToListAsync();
        return result;
    }

    public async Task<FavoriteFeature> DeleteById(int id)
    {
        var data = await GetById(id);
        await _favoriteFeatureRepository.Delete(data);
        return data;
    }

    public async Task<FavoriteFeature> GetById(int id)
    {
        return await _favoriteFeatureRepository.GetById(id);
    }

    public async Task<FavoriteFeature> Insert(FavoriteFeature model)
    {
        var data = await _favoriteFeatureRepository
            .Table.Where(s =>
                s.SubItemCode.Equals(model.SubItemCode)
                && s.FavoriteFeatureCode.Equals(model.FavoriteFeatureCode)
            )
            .FirstOrDefaultAsync();
        if (data == null)
        {
            await _favoriteFeatureRepository.Insert(model);
        }

        return model;
    }

    public async Task<FavoriteFeature> Update(FavoriteFeature model)
    {
        await _favoriteFeatureRepository.Update(model);
        return model;
    }
}
