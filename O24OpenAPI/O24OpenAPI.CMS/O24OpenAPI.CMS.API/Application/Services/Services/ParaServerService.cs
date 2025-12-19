using LinqToDB;
using O24OpenAPI.CMS.API.Application.Services.Interfaces;
using O24OpenAPI.CMS.Domain.AggregateModels;
using O24OpenAPI.Framework.Localization;

namespace O24OpenAPI.CMS.API.Application.Services.Services;

/// <summary>
/// ParaServer service
/// </summary>
public partial class ParaServerService(
    ILocalizationService localizationService,
    IRepository<ParaServer> ParaServerRepository
) : IParaServerService
{
    #region Fields
    private readonly ILocalizationService _localizationService = localizationService;
    private readonly IRepository<ParaServer> _ParaServerRepository = ParaServerRepository;

    #endregion
    #region Ctor

    #endregion
    /// <summary>
    /// Gets GetById
    /// </summary>
    /// <returns>Task&lt;ParaServer&gt;.</returns>
    public virtual async Task<ParaServer> GetById(int id)
    {
        return await _ParaServerRepository.GetById(id);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="app"></param>
    /// <param name="code"></param>
    /// <returns></returns>
    public virtual async Task<ParaServer> GetByAppAndCode(string app, string code)
    {
        return await _ParaServerRepository
            .Table.Where(s => s.App == app && s.Code == code)
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Gets GetBy Tx code And App
    /// </summary>
    /// <returns>Task&lt;ParaServerModel&gt;.</returns>
    public virtual async Task<List<ParaServer>> GetByApp(string app)
    {
        try
        {
            var getParaServer =
                await _ParaServerRepository.Table.Where(s => s.App.Equals(app)).ToListAsync()
                ?? throw new Exception(
                    await _localizationService.GetResource("CMS_ParaServer_ERR_0000000")
                );
            return getParaServer;
        }
        catch (Exception ex)
        {
            Console.WriteLine("GetByApp=Exception=getParaServer=" + ex.StackTrace);
        }
        return null;
    }

    /// <summary>
    ///Insert
    /// </summary>
    /// <returns>Task&lt;ParaServer&gt;.</returns>
    public virtual async Task Insert(ParaServer ParaServer)
    {
        await _ParaServerRepository.Insert(ParaServer);
    }

    /// <summary>
    ///Update
    /// </summary>
    /// <returns>Task&lt;ParaServer&gt;.</returns>
    public virtual async Task Update(ParaServer ParaServer)
    {
        await _ParaServerRepository.Update(ParaServer);
    }

    /// <summary>
    ///Delete.
    /// </summary>
    /// <returns>Task&lt;ParaServer&gt;.</returns>
    public virtual async Task<ParaServer> DeleteByAppAndParaServer(string app, string ParaServer)
    {
        await Task.CompletedTask;
        return null;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public virtual async Task<List<ParaServer>> GetAll()
    {
        return await _ParaServerRepository.Table.ToListAsync();
    }
}
