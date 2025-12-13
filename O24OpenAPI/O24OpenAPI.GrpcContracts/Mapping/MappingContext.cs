using LinKit.Core.Mapping;
using O24OpenAPI.APIContracts.Models.CTH;
using O24OpenAPI.APIContracts.Models.NCH;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Grpc.CTH;
using O24OpenAPI.Grpc.DTS;

namespace O24OpenAPI.GrpcContracts.Mapping;

[MapperContext]
public partial class MappingContext : IMappingConfigurator
{
    public void Configure(IMapperConfigurationBuilder builder)
    {
        builder
            .CreateMap<GetSMSLoanAlertReply, SMSLoanAlertModel>()
            .ForMember(
                d => d.DueDate,
                opt =>
                    opt.ConvertWith(
                        typeof(StringExtensions),
                        nameof(StringExtensions.ToDateTime),
                        src => src.DueDate
                    )
            )
            .ForMember(
                d => d.TotalPayment,
                o => o.MapFrom(src => decimal.Parse(src.TotalPayment))
            );
        ;
        builder.
            CreateMap<GetUserNotificationReply, CTHUserNotificationModel>();
        builder.CreateMap<UserCommandReply, CTHUserCommandModel>();
    }

}
