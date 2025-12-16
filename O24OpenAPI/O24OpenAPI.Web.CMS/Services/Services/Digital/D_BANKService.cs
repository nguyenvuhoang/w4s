using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.Framework.Infrastructure.Mapper.Extensions;
using O24OpenAPI.Framework.Localization;
using O24OpenAPI.Web.CMS.Services.Interfaces;

namespace O24OpenAPI.Web.CMS.Services.Services;

public partial class D_BANKService : ID_BANKService
{
    #region Fields

    private readonly ILocalizationService _localizationService;

    private readonly IRepository<D_BANK> _DbankApiRepository;

    #endregion

    #region Ctor

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="localizationService"></param>
    /// <param name="learnApiRepository"></param>
    public D_BANKService(
        ILocalizationService localizationService,
        IRepository<D_BANK> learnApiRepository
    )
    {
        _localizationService = localizationService;
        _DbankApiRepository = learnApiRepository;
    }

    #endregion

    /// <summary>
    /// Gets GetById
    /// </summary>
    /// <returns>Task&lt;learnApi&gt;.</returns>
    public virtual async Task<D_BANK> GetById(int id)
    {
        return await _DbankApiRepository.GetById(id);
    }

    /// <summary>
    /// Gets GetByTxcodeAndApp
    /// </summary>
    /// <returns>Task&lt;learnApiModel&gt;.</returns>
    public virtual async Task<D_BANKModel> GetByBin(string bin)
    {
        try
        {
            var getlearnApi = await _DbankApiRepository
                .Table.Where(s => s.Bin.Equals(bin.Trim()))
                .FirstOrDefaultAsync();

            if (getlearnApi == null)
            {
                return null;
            }

            return getlearnApi.ToModel<D_BANKModel>();
        }
        catch (System.Exception ex)
        {
            System.Console.WriteLine("GetByApp=Exception=getlearnApi=" + ex.StackTrace);
        }

        return null;
    }

    /// <summary>
    /// Gets SearchByApp
    /// </summary>
    /// <returns>Task&lt;learnApi&gt;.</returns>
    /// <summary>
    ///Insert
    /// </summary>
    /// <returns>Task&lt;learnApi&gt;.</returns>
    public virtual async Task<D_BANK> Insert(D_BANK learnApi)
    {
        var findForm = await _DbankApiRepository
            .Table.Where(s => s.Bin.Equals(learnApi.Bin))
            .FirstOrDefaultAsync();
        if (findForm == null)
        {
            await _DbankApiRepository.Insert(learnApi);
        }

        return learnApi;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public virtual async Task<List<D_BANK>> GetAll()
    {
        return await _DbankApiRepository.Table.Select(s => s).ToListAsync();
    }

    ///
    /// <summary>
    ///Update
    /// </summary>
    /// <returns>Task&lt;learnApi&gt;.</returns>
    public virtual async Task<D_BANK> Update(D_BANK learnApi)
    {
        await _DbankApiRepository.Update(learnApi);
        return learnApi;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="app"></param>
    /// <param name="learnApi"></param>
    /// <returns></returns>
    public virtual async Task<D_BANK> DeleteById(int id)
    {
        var learnApi = await GetById(id);
        await _DbankApiRepository.Delete(learnApi);
        return learnApi;
    }
}
