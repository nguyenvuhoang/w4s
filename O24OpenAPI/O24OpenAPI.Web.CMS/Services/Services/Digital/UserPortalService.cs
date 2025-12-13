using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.Web.CMS.Services.Interfaces.Digital;
using O24OpenAPI.Web.Framework.Localization;

namespace O24OpenAPI.Web.CMS.Services.Services.Digital;

public class UserPortalService : IUserPortalService
{
    private readonly ILocalizationService _localizationService;
    private readonly IRepository<S_USERPORTAL> _userPortalServiceRepo;

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="localizationService"></param>
    /// <param name="S_USERPORTALRepo"></param>
    public UserPortalService(
        ILocalizationService localizationService,
        IRepository<S_USERPORTAL> userPortalServiceRepo
    )
    {
        _localizationService = localizationService;
        _userPortalServiceRepo = userPortalServiceRepo;
    }

    public async Task<List<S_USERPORTAL>> GetAll()
    {
        return await _userPortalServiceRepo.Table.ToListAsync();
    }

    public async Task<S_USERPORTAL> DeleteById(int id)
    {
        var data = await GetById(id);
        await _userPortalServiceRepo.Delete(data);
        return data;
    }

    public async Task<S_USERPORTAL> GetById(int id)
    {
        return await _userPortalServiceRepo.GetById(id);
    }

    public async Task<S_USERPORTAL> Update(S_USERPORTAL model)
    {
        await _userPortalServiceRepo.Update(model);
        return model;
    }
}
