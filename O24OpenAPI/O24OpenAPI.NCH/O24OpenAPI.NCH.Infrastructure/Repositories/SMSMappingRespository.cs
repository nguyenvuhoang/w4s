using LinKit.Core.Abstractions;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.NCH.Domain.AggregatesModel.SmsAggregate;

namespace O24OpenAPI.NCH.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class SMSMappingRespository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
)
    : EntityRepository<SMSMappingResponse>(dataProvider, staticCacheManager),
        ISMSMappingRespository
{
}
