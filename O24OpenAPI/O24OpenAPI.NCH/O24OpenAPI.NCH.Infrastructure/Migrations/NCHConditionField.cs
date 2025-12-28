using O24OpenAPI.Core.Domain;
using O24OpenAPI.NCH.Domain;
using O24OpenAPI.Framework.Domain;
using O24OpenAPI.NCH.Domain.AggregatesModel.SmsAggregate;
using O24OpenAPI.NCH.Domain.AggregatesModel.MailAggregate;

namespace O24OpenAPI.NCH.Migrations;

public class NCHConditionField
{
    public static readonly List<string> SMSProviderCondition =
    [
         nameof(SMSProvider.ProviderName)
    ];

    public static readonly List<string> SMSProviderConfigCondition =
    [
         nameof(SMSProviderConfig.SMSProviderId) ,
         nameof(SMSProviderConfig.ConfigKey),
         nameof(SMSProviderConfig.ConfigValue)
    ];

    public static readonly List<string> O24OpenAPIServiceCondition =
    [
         nameof(O24OpenAPIService.StepCode)
    ];
    public static readonly List<string> ScheduleTaskCondition =
    [
         nameof(ScheduleTask.Name)
    ];

    public static readonly List<string> StoredCommandCondition =
    [
        nameof(StoredCommand.Name)
    ];

    public static readonly List<string> SMSMappingResponseCondition =
    [
           nameof(SMSMappingResponse.ProviderName),
           nameof(SMSMappingResponse.ResponseCode),
    ];
    public static readonly List<string> MailTemplateCondition =
    [
          nameof(MailTemplate.TemplateId),
    ];
    public static readonly List<string> MailConfigCondition =
    [
         nameof(MailConfig.ConfigId),
    ];
    public static readonly List<string> SMSTemplateCondition =
    [
         nameof(SMSTemplate.TemplateCode),
    ];

}
