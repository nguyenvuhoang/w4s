using Newtonsoft.Json.Linq;
using O24OpenAPI.Framework.Services;
using O24OpenAPI.Web.CMS.Services.Interfaces;

namespace O24OpenAPI.Web.CMS.Services.Services;

public partial class UserAccountService : IUserAccountService
{
    #region Fields
    private readonly IStoredCommandService _storedCommandService;
    private readonly IMappingService _mappingService;
    private readonly IO9ClientService _o9ClientService;
    private readonly IWorkflowStepService _workflowStepService;

    public UserAccountService(
        IStoredCommandService storedCommandService,
        IO9ClientService o9ClientService,
        IWorkflowStepService workflowStepService,
        IMappingService mappingService
    )
    {
        this._storedCommandService = storedCommandService;
        this._o9ClientService = o9ClientService;
        this._workflowStepService = workflowStepService;
        this._mappingService = mappingService;
    }

    #endregion
    public async Task<JToken> GetStaffCareInfo(string userId)
    {
        var storedCommand = await _storedCommandService.GetByName("GetStaffCareInforByUserId");
        if (storedCommand == null)
        {
            throw new Exception("Stored Command GetStaffCareInforByUserId not found");
        }
        string sql = storedCommand.Query;
        sql = string.Format(sql, userId);
        var result = await _o9ClientService.SearchAsync(sql);

        if (result["data"] is JArray dataArray && dataArray.Count > 0)
        {
            return result;
        }

        return null;
    }

    public async Task<JToken> GetStaffCareList()
    {
        var storedCommand = await _storedCommandService.GetByName("GetStaffCareInforList");
        if (storedCommand == null)
        {
            throw new Exception("Stored Command GetStaffCareInforList not found");
        }
        string sql = storedCommand.Query;
        sql = string.Format(sql);
        var result = await _o9ClientService.SearchAsync(sql);

        if (result["data"] is JArray dataArray && dataArray.Count > 0)
        {
            return result;
        }

        return null;
    }

    public async Task<JToken> GetUserAccountList()
    {
        var storedCommand = await _storedCommandService.GetByName("GetUserAccountList");
        if (storedCommand == null)
        {
            throw new Exception("Stored Command GetUserAccountList not found");
        }
        string sql = storedCommand.Query;
        sql = string.Format(sql);
        var result = await _o9ClientService.SearchAsync(sql);

        if (result["data"] is JArray dataArray && dataArray.Count > 0)
        {
            return result;
        }

        return null;
    }
}
