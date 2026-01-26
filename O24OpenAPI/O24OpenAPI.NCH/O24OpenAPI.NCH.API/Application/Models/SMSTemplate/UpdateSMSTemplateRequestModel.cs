using O24OpenAPI.Framework.Models;
using System.Reflection;
using SmsDomain = O24OpenAPI.NCH.Domain.AggregatesModel.SmsAggregate;

namespace O24OpenAPI.NCH.API.Application.Models.SMSTemplate;

public class UpdateSMSTemplateRequestModel : BaseTransactionModel
{
    public UpdateSMSTemplateRequestModel() { }

    public int Id { get; set; }
    public string TemplateCode { get; set; } = string.Empty;
    public string MessageContent { get; set; } = string.Empty;
    public bool? IsActive { get; set; }
    public DateTime? CreatedOnUtc { get; set; }
    public DateTime? UpdatedOnUtc { get; set; }
    public List<string> ChangedFields { get; set; } = [];

    public static UpdateSMSTemplateRequestModel FromUpdatedEntity(
        SmsDomain.SMSTemplate updated,
        SmsDomain.SMSTemplate original
    )
    {
        UpdateSMSTemplateRequestModel result = new();
        PropertyInfo[] entityProps = typeof(SmsDomain.SMSTemplate).GetProperties();
        Dictionary<string, PropertyInfo> modelProps = typeof(UpdateSMSTemplateRequestModel)
            .GetProperties()
            .ToDictionary(p => p.Name);

        foreach (PropertyInfo prop in entityProps)
        {
            if (!modelProps.ContainsKey(prop.Name))
            {
                continue;
            }

            object newValue = prop.GetValue(updated);
            object oldValue = prop.GetValue(original);

            if (
                (oldValue == null && newValue != null)
                || (oldValue != null && !oldValue.Equals(newValue))
            )
            {
                result.ChangedFields.Add(prop.Name);
            }

            modelProps[prop.Name].SetValue(result, newValue);
        }

        return result;
    }
}
