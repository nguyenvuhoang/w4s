using O24OpenAPI.Framework.Domain.Logging;
using O24OpenAPI.Framework.Infrastructure.Mapper;
using O24OpenAPI.Logger.API.Models.HttpLog;
using O24OpenAPI.Logger.API.Models.ServiceLog;
using O24OpenAPI.Logger.API.Models.Workflow;
using O24OpenAPI.Logger.Domain.AggregateModels.ServiceLogAggregate;
using O24OpenAPI.Logger.Domain.AggregateModels.WorkflowLogAggregate;

namespace O24OpenAPI.Logger.API.Infrastructure.Mapper;

/// <summary>
/// The mapper config class
/// </summary>
/// <seealso cref="BaseMapperConfiguration"/>
public class MapperConfig : BaseMapperConfiguration
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MapperConfig"/> class
    /// </summary>
    public MapperConfig()
    {
        CreateModelMap<ServiceLog, ServiceLogSearchResponse>();
        CreateModelMap<HttpLog, HttpLogSearchResponse>();
        CreateModelMap<WorkflowLog, WorkflowLogSearchResponse>();
        CreateModelMap<WorkflowStepLog, WorkflowStepLogSearchResponse>();
    }
}
