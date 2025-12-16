using LinqToDB;
using O24OpenAPI.Web.CMS.Services.Interfaces.Digital;
using O24OpenAPI.Framework.Localization;

namespace O24OpenAPI.Web.CMS.Services.Services.Digital;

public class FavoriteFeatureSubItemService : IFavoriteFeatureSubItemService
{
    private readonly ILocalizationService _localizationService;
    private readonly IRepository<FavoriteFeatureSubItem> _favoriteFeatureSubItemRepo;

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="localizationService"></param>
    /// <param name="favoriteFeatureRepo"></param>
    public FavoriteFeatureSubItemService(
        ILocalizationService localizationService,
        IRepository<FavoriteFeatureSubItem> favoriteFeatureRepo
    )
    {
        _localizationService = localizationService;
        _favoriteFeatureSubItemRepo = favoriteFeatureRepo;
    }

    public async Task<List<FavoriteFeatureSubItem>> GetAll()
    {
        return await _favoriteFeatureSubItemRepo.Table.ToListAsync();
    }

    public async Task<FavoriteFeatureSubItem> DeleteById(int id)
    {
        var data = await GetById(id);

        if (data != null)
        {
            await _favoriteFeatureSubItemRepo.Delete(data);
        }

        return data;
    }

    public async Task<FavoriteFeatureSubItem> GetById(int id)
    {
        return await _favoriteFeatureSubItemRepo.GetById(id);
    }

    public async Task<FavoriteFeatureSubItem> Insert(FavoriteFeatureSubItem model)
    {
        var data = await _favoriteFeatureSubItemRepo
            .Table.Where(s =>
                s.SubItemCode.Equals(model.SubItemCode)
                && s.SubItemCode.Equals(model.SubItemCode)
            )
            .FirstOrDefaultAsync();
        if (data == null)
        {
            await _favoriteFeatureSubItemRepo.Insert(model);
        }
        else
        {
            throw new O24OpenAPIException("Common.Value.Unique", model.SubItemCode);
        }
        return model;
    }

    public async Task<FavoriteFeatureSubItem> Update(FavoriteFeatureSubItem model)
    {
        await _favoriteFeatureSubItemRepo.Update(model);
        return model;
    }
}
