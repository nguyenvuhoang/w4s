using O24OpenAPI.CMS.Domain.AggregateModels;
using O24OpenAPI.CMS.Domain.AggregateModels.FormAggregate;
using O24OpenAPI.CMS.Domain.AggregateModels.LearnApiAggregate;
using O24OpenAPI.Core.Configuration;
using O24OpenAPI.Framework.Domain;

namespace O24OpenAPI.CMS.API.Application.Migrations;

/// <summary>
/// The cth condition field class
/// </summary>
public class CMSConditionField
{
    public static readonly List<string> CoreApiKeysCondition =
    [
        nameof(CoreApiKeys.ClientId),
        nameof(CoreApiKeys.ClientSecret),
        nameof(CoreApiKeys.BICCode),
    ];
    public static readonly List<string> LearnApiCondition =
    [
        nameof(LearnApi.LearnApiId),
        nameof(LearnApi.Channel),
    ];
    public static readonly List<string> O24OpenAPIServiceCondition =
    [
        nameof(O24OpenAPIService.StepCode),
    ];
    public static readonly List<string> FormCondition = [nameof(Form.FormId), nameof(Form.App)];
    public static readonly List<string> TranslationLanguagesCondition =
    [
        nameof(TranslationLanguages.ChannelId),
        nameof(TranslationLanguages.Language),
    ];
    public static readonly List<string> SettingCondition = [nameof(Setting.Name)];
}
