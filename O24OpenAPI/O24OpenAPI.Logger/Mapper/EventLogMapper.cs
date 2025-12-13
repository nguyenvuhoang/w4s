using O24OpenAPI.Contracts.Models.Log;
using O24OpenAPI.Logger.Domain;
using Riok.Mapperly.Abstractions;

namespace O24OpenAPI.Logger.Mapper;

[Mapper(
    RequiredMappingStrategy = RequiredMappingStrategy.None,
    RequiredEnumMappingStrategy = RequiredMappingStrategy.None
)]
public static partial class EventLogMapper
{
    public static partial ApplicationLog MapToApplicationLog(this LogEntryModel model);
}
