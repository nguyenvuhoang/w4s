using O24OpenAPI.Framework.Domain.Logging;
using O24OpenAPI.Framework.Infrastructure.Mapper;
using O24OpenAPI.Logger.Domain;
using O24OpenAPI.Logger.Models.HttpLog;
using O24OpenAPI.Logger.Models.ServiceLog;
using O24OpenAPI.Logger.Models.Workflow;

namespace O24OpenAPI.Logger.Infrastructure.Mapper;

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
