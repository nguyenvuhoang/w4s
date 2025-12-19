using LinqToDB;
using O24OpenAPI.Client.Log;
using O24OpenAPI.CMS.API.Application.Models;
using O24OpenAPI.CMS.API.Application.Services.Interfaces;
using O24OpenAPI.Framework.Infrastructure.Mapper.Extensions;

namespace O24OpenAPI.CMS.API.Application.Services.Services;

public partial class LogServiceService : ILogServiceService
{
    private readonly IRepository<LogService> _LogServiceRepository;

    #region Ctor
    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="localizationService"></param>
    /// <param name="LogServiceRepository"></param>
    public LogServiceService(IRepository<LogService> LogServiceRepository)
    {
        _LogServiceRepository = LogServiceRepository;
    }

    #endregion
    /// <summary>
    /// Gets GetById
    /// </summary>
    /// <returns>Task&lt;LogService&gt;.</returns>
    public virtual async Task<LogService> GetById(int id)
    {
        return await _LogServiceRepository.GetById(id);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="logService"></param>
    /// <returns></returns>
    public virtual LogServiceModel ToLogServiceModel(LogService logService)
    {
        var model_ = logService.ToModel<LogServiceModel>();
        model_.LogUtc = logService.LogUtc;
        model_.Date = Utils.Utils.ConvertFromUnixTimestamp(logService.LogUtc).ToShortDateString();
        model_.Time = Utils
            .Utils.ConvertFromUnixTimestamp(logService.LogUtc)
            .ToString("hh:mm:ss.fffffff");
        return model_;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="centralizedLog"></param>
    /// <returns></returns>
    public virtual async Task WriteCentralizedLog(CentralizedLog centralizedLog)
    {
        await _LogServiceRepository.Insert(
            new LogService()
            {
                LogUtc = centralizedLog.log_utc,
                ExecutionId = centralizedLog.execution_id,
                ServiceId = centralizedLog.service_id,
                StepCode = centralizedLog.step_code,
                StepExecutionId = centralizedLog.step_execution_id,
                Subject = centralizedLog.subject,
                LogText = centralizedLog.log_text,
                JsonDetails = centralizedLog.json_details,
                LogType = centralizedLog.log_type,
            }
        );
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public virtual async Task<List<LogServiceModel>> GetAll()
    {
        return await _LogServiceRepository
            .Table.OrderByDescending(d => d.LogUtc)
            .Select(s => ToLogServiceModel(s))
            .ToListAsync();
    }

    /// <summary>
    /// Gets SearchByApp
    /// </summary>
    /// <returns>Task&lt;LogService&gt;.</returns>
    public virtual async Task WriteLog(
        string subject,
        string logText,
        string details,
        string logType
    )
    {
        await Task.CompletedTask;
        var workflow = new WorkflowScheme();
        // JITS.NeptuneClient.Scheme.Workflow.WorkflowScheme.CentralizedLogType logType_;
        // Enum.TryParse(logType, true, out logType_);
        // var theCentralizedLog = workflow.CreateCentralizedLog(subject, logText, details, logType_);
        var theCentralizedLog = WorkflowScheme.CreateCentralizedLog(subject, logText);

        await WriteCentralizedLog(theCentralizedLog);
    }

    /// <summary>
    ///Insert
    /// </summary>
    /// <returns>Task&lt;LogService&gt;.</returns>
    public virtual async Task Insert(LogService logService)
    {
        await _LogServiceRepository.Insert(logService);
    }

    /// <summary>
    ///Update
    /// </summary>
    /// <returns>Task&lt;LogService&gt;.</returns>
    public virtual async Task<LogService> Update(LogService LogService)
    {
        await Task.CompletedTask;
        return null;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public virtual void DeleteAll()
    {
        _LogServiceRepository.Truncate();
    }
}
