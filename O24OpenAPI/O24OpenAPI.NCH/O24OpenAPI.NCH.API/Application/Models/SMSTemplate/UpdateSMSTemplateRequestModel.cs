using O24OpenAPI.Framework.Models;
using O24OpenAPI.NCH.Domain.AggregatesModel.SmsAggregate;
using SmsDomain = O24OpenAPI.NCH.Domain.AggregatesModel.SmsAggregate;
namespace O24OpenAPI.NCH.Models.SMSTemplate;

public class UpdateSMSTemplateRequestModel : BaseTransactionModel
{
    public UpdateSMSTemplateRequestModel() { }

    public int Id { get; set; }
    public string TemplateCode { get; set; } = string.Empty;
    public string MessageContent { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool? IsActive { get; set; }

    public DateTime? CreatedOnUtc { get; set; }

    public DateTime? UpdatedOnUtc { get; set; }

    public List<string> ChangedFields { get; set; } = [];

    public static UpdateSMSTemplateRequestModel FromUpdatedEntity(
        SmsDomain.SMSTemplate updated,
        SmsDomain.SMSTemplate original
    )
    {
        var result = new UpdateSMSTemplateRequestModel();
        var entityProps = typeof(SmsDomain.SMSTemplate).GetProperties();
        var modelProps = typeof(UpdateSMSTemplateRequestModel)
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
