using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.NCH.Domain.AggregatesModel.SmsAggregate;
using SmsDomain = O24OpenAPI.NCH.Domain.AggregatesModel.SmsAggregate;

namespace O24OpenAPI.NCH.Models.SMSTemplate;

public class UpdateSMSTemplateResponseModel : BaseO24OpenAPIModel
{
    public UpdateSMSTemplateResponseModel() { }

    public string TemplateCode { get; set; } = string.Empty;
    public string MessageContent { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool? IsActive { get; set; }

    public DateTime? CreatedOnUtc { get; set; }

    public DateTime? UpdatedOnUtc { get; set; }
    public List<string> ChangedFields { get; set; } = [];

    public static UpdateSMSTemplateResponseModel FromUpdatedEntity(
        SmsDomain.SMSTemplate updated,
        SmsDomain.SMSTemplate original
    )
    {
        var result = new UpdateSMSTemplateResponseModel();
        var entityProps = typeof(SmsDomain.SMSTemplate).GetProperties();
        var modelProps = typeof(UpdateSMSTemplateResponseModel)
            .GetProperties()
            .ToDictionary(p => p.Name);

        foreach (var prop in entityProps)
        {
            if (!modelProps.ContainsKey(prop.Name))
            {
                continue;
            }

            var newValue = prop.GetValue(updated);
            var oldValue = prop.GetValue(original);

            bool isChanged =
                (oldValue == null && newValue != null)
                || (oldValue != null && !oldValue.Equals(newValue));

            if (isChanged)
            {
                result.ChangedFields.Add(prop.Name);
            }

            var targetProp = modelProps[prop.Name];
            if (newValue != null && targetProp.PropertyType != newValue.GetType())
            {
                try
                {
                    var targetType =
                        Nullable.GetUnderlyingType(targetProp.PropertyType)
                        ?? targetProp.PropertyType;
                    newValue = Convert.ChangeType(newValue, targetType);
                }
                catch
                {
                    continue;
                }
            }

            targetProp.SetValue(result, newValue);
        }

        return result;
    }
}
