using O24OpenAPI.O24NCH.Domain;
using O24OpenAPI.Web.Framework.Models;

namespace O24OpenAPI.O24NCH.Models.Request;

public class SMSProviderUpdateModel : BaseTransactionModel
{
    public SMSProviderUpdateModel() { }
    public int Id { get; set; }
    public string ApiUrl { get; set; }
    public string CountryPrefix { get; set; }
    public string AllowedPrefix { get; set; }
    public string ApiUsername { get; set; }
    public string ApiPassword { get; set; }
    public string ApiKey { get; set; }
    public string BrandName { get; set; }
    public bool? IsActive { get; set; }
    public List<SMSProviderConfigModel> SMSProviderConfig { get; set; }
    public List<string> ChangedFields { get; set; } = [];
    public static SMSProviderUpdateModel FromUpdatedEntity(SMSProvider updated, SMSProvider original)
    {
        var result = new SMSProviderUpdateModel();
        var entityProps = typeof(SMSProvider).GetProperties();
        var modelProps = typeof(SMSProviderUpdateModel).GetProperties().ToDictionary(p => p.Name);

        foreach (var prop in entityProps)
        {
            if (!modelProps.ContainsKey(prop.Name))
            {
                continue;
            }

            var newValue = prop.GetValue(updated);
            var oldValue = prop.GetValue(original);

            if ((oldValue == null && newValue != null) ||
                (oldValue != null && !oldValue.Equals(newValue)))
            {
                result.ChangedFields.Add(prop.Name);
            }

            modelProps[prop.Name].SetValue(result, newValue);
        }

        return result;
    }
}
