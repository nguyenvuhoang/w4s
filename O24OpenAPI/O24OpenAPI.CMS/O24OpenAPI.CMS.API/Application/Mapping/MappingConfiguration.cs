using LinKit.Core.Mapping;
using O24OpenAPI.CMS.API.Application.LearnApis;
using O24OpenAPI.CMS.API.Application.Models;
using O24OpenAPI.CMS.API.Models.VNPay;
using O24OpenAPI.CMS.Domain.AggregateModels.LearnApiAggregate;
using O24OpenAPI.GrpcClient.Generated;

namespace O24OpenAPI.CMS.API.Application.Mapping;

[MapperContext]
public class MappingConfiguration : IMappingConfigurator
{
    public void Configure(IMapperConfigurationBuilder builder)
    {
        builder.CreateMap<LearnApi, LearnApiModel>();
        builder.CreateMap<AddLearnApiCommand, LearnApi>();
        builder.CreateMap<VNPayProcessPayModel, PMTGrpcClientServiceVNPayProcessPayCommand>()
         .ForMember(
            d => d.Amount,
            opt => opt.MapFrom(s => s.Amount.ToString())
        );
        builder.CreateMap<VNPayProcessReturnModel, PMTGrpcClientServiceVNPayProcessReturnCommand>();
    }
}
