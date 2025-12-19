using System.Diagnostics;
using LinqToDB;
using Newtonsoft.Json;
using O24OpenAPI.Client.Workflow;
using O24OpenAPI.Core;
using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Framework.Helpers;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.WFO.Domain;
using O24OpenAPI.WFO.Models;
using O24OpenAPI.WFO.Services.Interfaces;
using WorkflowInput = O24OpenAPI.WFO.Models.WorkflowInput;

namespace O24OpenAPI.WFO.Services;

public class WorkflowService(
    IRepository<WorkflowDef> workflowDefRepository,
    IRepository<WorkflowStep> workflowStepRepository,
    IStaticCacheManager staticCacheManager,
    IWorkflowDefService workflowDefService,
    IWorkflowStepService workflowStepService
) : IWorkflowService
{
    private readonly IRepository<WorkflowDef> _workflowDefRepository = workflowDefRepository;
    private readonly IRepository<WorkflowStep> _workflowStepRepository = workflowStepRepository;
    private readonly IStaticCacheManager _staticCacheManager = staticCacheManager;
    private readonly IWorkflowDefService _workflowDefService = workflowDefService;
    private readonly IWorkflowStepService _workflowStepService = workflowStepService;

    public async Task<WorkflowDef> GetWorkflowDef(string workflowId, string channelId)
    {
        return await _workflowDefRepository.Table.FirstOrDefaultAsync(x =>
            x.WorkflowId == workflowId && x.ChannelId == channelId
        );
    }

    public async Task<List<WorkflowStep>> GetWorkflowSteps(string workflowId)
    {
        try
        {
            return await _workflowStepRepository
                .Table.Where(x => x.WorkflowId == workflowId && x.Status)
                .OrderBy(x => x.StepOrder)
                .ToListAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<(WorkflowDef, List<WorkflowStep>)> GetExecutingWorkflow(
        string workflowId,
        string channelId
    )
    {
        var workflow =
            await _staticCacheManager.GetOrSetAsync(
                new CacheKey($"workflow_def_{channelId}_{workflowId}"),
                () => GetWorkflowDef(workflowId, channelId)
            )
            ?? throw new InvalidOperationException(
                $"Invalid workflow [{channelId}] [{workflowId}]"
            );

        var steps =
            await _staticCacheManager.GetOrSetAsync(
                new CacheKey($"workflow_steps_{channelId}_{workflowId}"),
                () => GetWorkflowSteps(workflow.WorkflowId)
            )
            ?? throw new InvalidOperationException(
                $"Invalid workflow steps [{channelId}] [{workflowId}]"
            );
        return (workflow, steps);
    }

    public async Task<long> GetMaxTimeOut()
    {
        var maxTimeout = await _workflowDefRepository.Table.MaxAsync(x => x.Timeout);
        return maxTimeout > 0 ? maxTimeout : 60000;
    }

    public async Task<WorkflowResponse> CreateWorkflow(WorkflowInput input)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var response = new WorkflowResponse
        {
            execution_id = input.ExecutionId,
            transaction_date = input.TransactionDate,
        };
        try
        {
            if (!input.Fields.TryGetValue("def", out object defObject))
            {
                throw new InvalidOperationException("Invalid workflow definition");
            }
            if (!input.Fields.TryGetValue("steps", out object stepsObject))
            {
                throw new InvalidOperationException("Invalid workflow steps");
            }
            var wfDef =
                JsonConvert.DeserializeObject<WorkflowDef>(defObject.ToSerialize())
                ?? throw new InvalidOperationException("Invalid workflow definition");
            var exit = await _workflowDefService.GetByWorkflowIdAsync(
                wfDef.WorkflowId,
                wfDef.ChannelId
            );
            if (exit != null)
            {
                throw new InvalidOperationException(
                    $"Workflow [{wfDef.WorkflowId}] already exits."
                );
            }
            var wfSteps = JsonConvert.DeserializeObject<List<WorkflowStep>>(
                stepsObject.ToSerialize()
            );
            if (wfSteps == null || wfSteps.Count == 0)
            {
                throw new InvalidOperationException("Invalid workflow steps");
            }
            await _workflowDefRepository.InsertAsync(wfDef);
            foreach (var step in wfSteps)
            {
                step.WorkflowId = wfDef.WorkflowId;
                await _workflowStepRepository.InsertAsync(step);
            }
            stopwatch.Stop();

            response.data["data"] = true;
            response.time_in_milliseconds = stopwatch.ElapsedMilliseconds;
            response.status = Enum_STATUS.Completed.ToString();
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            response.data["data"] = false;
            response.time_in_milliseconds = stopwatch.ElapsedMilliseconds;
            response.status = Enum_STATUS.Error.ToString();
            response.error_message = ex.Message;
            response.error_code = ex.HResult.ToString("X8");
        }
        return response;
    }

    public async Task<WorkflowResponse> UpdateWorkflow(WorkflowInput input)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var response = new WorkflowResponse
        {
            execution_id = input.ExecutionId,
            transaction_date = input.TransactionDate,
        };
        try
        {
            if (!input.Fields.TryGetValue("def", out object defObject))
            {
                throw new InvalidOperationException("Invalid workflow definition");
            }
            if (!input.Fields.TryGetValue("steps", out object stepsObject))
            {
                throw new InvalidOperationException("Invalid workflow steps");
            }
            var wfDef =
                JsonConvert.DeserializeObject<WorkflowDef>(defObject.ToSerialize())
                ?? throw new InvalidOperationException("Invalid workflow definition");
            var exit =
                await _workflowDefService.GetByWorkflowIdAsync(wfDef.WorkflowId, wfDef.ChannelId)
                ?? throw new InvalidOperationException(
                    $"Workflow [{wfDef.WorkflowId}] does not exit."
                );

            await _workflowDefService.DeleteAsync(exit);
            wfDef = await _workflowDefService.AddAsync(wfDef);
            var wfSteps = JsonConvert.DeserializeObject<List<WorkflowStep>>(
                stepsObject.ToSerialize()
            );
            if (wfSteps == null || wfSteps.Count == 0)
            {
                throw new InvalidOperationException("Invalid workflow steps");
            }
            await _workflowStepService.DeleteByWorkflowIdAsync(wfDef.WorkflowId);

            foreach (var step in wfSteps)
            {
                step.WorkflowId = wfDef.WorkflowId;
                await _workflowStepRepository.InsertAsync(step);
            }
            stopwatch.Stop();

            response.data["data"] = true;
            response.time_in_milliseconds = stopwatch.ElapsedMilliseconds;
            response.status = Enum_STATUS.Completed.ToString();
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            response.data["data"] = false;
            response.time_in_milliseconds = stopwatch.ElapsedMilliseconds;
            response.status = Enum_STATUS.Error.ToString();
            response.error_message = ex.Message;
            response.error_code = ex.HResult.ToString("X8");
        }
        return response;
    }

    public async Task<WorkflowResponse> SimpleSearch(WorkflowInput input)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var response = new WorkflowResponse
        {
            execution_id = input.ExecutionId,
            transaction_date = input.TransactionDate,
        };
        try
        {
            var searchModel = JsonConvert.DeserializeObject<SimpleSearchModel>(
                input.Fields.ToSerialize()
            );
            var result = await _workflowDefService.SimpleSearch(searchModel);
            var pageListModel = result.ToPagedListModel<WorkflowDef, WorkflowDefSearchResponse>();
            stopwatch.Stop();

            response.data = pageListModel.ToDictionary();
            response.time_in_milliseconds = stopwatch.ElapsedMilliseconds;
            response.status = Enum_STATUS.Completed.ToString();
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            response.data = [];
            response.time_in_milliseconds = stopwatch.ElapsedMilliseconds;
            response.status = Enum_STATUS.Error.ToString();
            response.error_message = ex.Message;
            response.error_code = ex.HResult.ToString("X8");
        }
        return response;
    }

    public async Task<WorkflowResponse> DeleteWorkflow(WorkflowInput input)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var response = new WorkflowResponse
        {
            execution_id = input.ExecutionId,
            transaction_date = input.TransactionDate,
        };
        try
        {
            if (!input.Fields.TryGetValue("id", out object idOb))
            {
                throw new O24OpenAPIException("Id is required!");
            }
            int id = idOb.ToInt();
            var wfDef =
                await _workflowDefService.GetByIdAsync(id)
                ?? throw new InvalidOperationException($"Workflow with id [{id}] does not exit.");
            await _workflowDefService.DeleteAsync(wfDef);
            await _workflowStepService.DeleteByWorkflowIdAsync(wfDef.WorkflowId);
            stopwatch.Stop();

            response.data["data"] = true;
            response.time_in_milliseconds = stopwatch.ElapsedMilliseconds;
            response.status = Enum_STATUS.Completed.ToString();
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            response.data["data"] = false;
            response.time_in_milliseconds = stopwatch.ElapsedMilliseconds;
            response.status = Enum_STATUS.Error.ToString();
            response.error_message = ex.Message;
            response.error_code = ex.HResult.ToString("X8");
        }
        return response;
    }
}
