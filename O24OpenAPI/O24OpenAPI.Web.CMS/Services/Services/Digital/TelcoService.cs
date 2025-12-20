using LinqToDB;
using O24OpenAPI.Framework.Localization;
using O24OpenAPI.Web.CMS.Models.Digital;
using O24OpenAPI.Web.CMS.Services.Interfaces;

namespace O24OpenAPI.Web.CMS.Services.Services;

public class TelcoService : ITelcoService
{
    private readonly ILocalizationService _localizationService;

    private readonly IRepository<D_TOP_TELCO> _telcoRepository;

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="localizationService"></param>
    /// <param name="telcoRepository"></param>
    public TelcoService(
        ILocalizationService localizationService,
        IRepository<D_TOP_TELCO> telcoRepository
    )
    {
        _localizationService = localizationService;
        _telcoRepository = telcoRepository;
    }

    /// <summary>
    /// Gets GetById
    /// </summary>
    /// <param name="TelcoName">The id</param>
    /// <returns>Task&lt;Bo&gt;.</returns>
    public virtual async Task<D_TOP_TELCO> GetByTelcoName(string TelcoName)
    {
        return await _telcoRepository
            .Table.Where(s => s.TelcoName == TelcoName)
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Gets GetById
    /// </summary>
    /// <param name="TelcoID">The id</param>
    /// <returns>Task&lt;Bo&gt;.</returns>
    public virtual async Task<D_TOP_TELCO> GetByTelcoID(int TelcoID)
    {
        return await _telcoRepository.Table.Where(s => s.Id == TelcoID).FirstOrDefaultAsync();
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public virtual async Task<List<D_TOP_TELCO>> GetAll()
    {
        return await _telcoRepository.Table.Select(s => s).ToListAsync();
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public virtual async Task<List<D_TOP_TELCO>> Search(SearchTelcoModel model)
    {
        var listTelco = await (
            from d in _telcoRepository.Table
            where
                (
                    (!string.IsNullOrEmpty(model.TelcoName) && model.TelcoName == d.TelcoName)
                    || string.IsNullOrEmpty(model.TelcoName)
                )
                && (
                    (!string.IsNullOrEmpty(model.ShortName) && model.ShortName == d.ShortName)
                    || string.IsNullOrEmpty(model.ShortName)
                )
                && (
                    (
                        !string.IsNullOrEmpty(model.ELoadBillerCode)
                        && model.ELoadBillerCode == d.ELoadBillerCode
                    ) || string.IsNullOrEmpty(model.ELoadBillerCode)
                )
                && (
                    (
                        !string.IsNullOrEmpty(model.EPinBillerCode)
                        && model.EPinBillerCode == d.EPinBillerCode
                    ) || string.IsNullOrEmpty(model.EPinBillerCode)
                )
                && (
                    (
                        !string.IsNullOrEmpty(model.SUNDRYACCTNOBANK)
                        && model.SUNDRYACCTNOBANK == d.SUNDRYACCTNOBANK
                    ) || string.IsNullOrEmpty(model.SUNDRYACCTNOBANK)
                )
                && (
                    (
                        !string.IsNullOrEmpty(model.SUNDRYACCTNOWALLET)
                        && model.ELoadBillerCode == d.SUNDRYACCTNOWALLET
                    ) || string.IsNullOrEmpty(model.SUNDRYACCTNOWALLET)
                )
                && (
                    (!string.IsNullOrEmpty(model.Status) && model.Status == d.Status)
                    || string.IsNullOrEmpty(model.Status)
                )
            select d
        ).ToListAsync();

        return listTelco;
    }

    /// <summary>
    ///Insert
    /// </summary>
    /// <returns>Task&lt;Bo&gt;.</returns>
    public virtual async Task<D_TOP_TELCO> Insert(D_TOP_TELCO telco)
    {
        var findTelco = await _telcoRepository
            .Table.Where(s => s.TelcoName.Equals(telco.TelcoName))
            .FirstOrDefaultAsync();
        if (findTelco == null)
        {
            await _telcoRepository.Insert(telco);
        }
        else
        {
            throw new O24OpenAPIException(
                "InvalidTelco",
                "The Telco code already existing in system"
            );
        }

        return telco;
    }

    /// <summary>
    ///Update
    /// </summary>
    /// <returns>Task&lt;Bo&gt;.</returns>
    public virtual async Task<D_TOP_TELCO> Update(D_TOP_TELCO telco)
    {
        await _telcoRepository.Update(telco);
        return telco;
    }

    /// <summary>
    ///Delete.
    /// </summary>
    /// <returns>Task&lt;Bo&gt;.</returns>
    public virtual async Task<D_TOP_TELCO> DeleteById(int TelcoID)
    {
        var telco = await _telcoRepository.Table.Where(s => s.Id == TelcoID).FirstOrDefaultAsync();

        if (telco == null)
        {
            throw new O24OpenAPIException(
                "InvalidTelcoID",
                "The telco code does not exist in system"
            );
        }
        await _telcoRepository.Delete(telco);
        return telco;
    }
}
