// using O24OpenAPI.O24DTS.Domain; // nếu CoreApiKeys của bạn nằm namespace khác, đổi import này cho khớp

using O24OpenAPI.CMS.Domain.AggregateModels;

namespace O24OpenAPI.CMS.API.Application.Models.OpenAPI;

public partial class OpenAPIQueueResponseModel : BaseO24OpenAPIModel
{
    public OpenAPIQueueResponseModel() { }

    public int Id { get; set; }

    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;

    public string BICCode { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Environment { get; set; } = string.Empty;
    public string Scopes { get; set; } = string.Empty;
    public DateTime? ExpiredOnUtc { get; set; }
    public bool? IsRevoked { get; set; } = false;
    public bool? IsActive { get; set; } = true;
    public DateTime? CreatedOnUtc { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? LastUsedOnUtc { get; set; }
    public int? UsageCount { get; set; } = 0;
    public int? AccessTokenTtlSeconds { get; set; } = 0;
    public int? AccessTokenMaxTtlSeconds { get; set; } = 0;
    public int? AccessTokenMaxUses { get; set; } = 0;
    public string AccessTokenTrustedIPs { get; set; } = string.Empty;
    public string ClientSecretTrustedIPs { get; set; } = string.Empty;
    public string ClientSecretDescription { get; set; } = string.Empty;
    public DateTime? ClientSecretExpiresOnUtc { get; set; }

    // ===== Audit =====
    public List<string> ChangedFields { get; set; } = [];

    public static OpenAPIQueueResponseModel FromUpdatedEntity(
        CoreApiKeys updated,
        CoreApiKeys original
    )
    {
        var result = new OpenAPIQueueResponseModel();

        var entityProps = typeof(CoreApiKeys).GetProperties();
        var modelProps = typeof(OpenAPIQueueResponseModel)
            .GetProperties()
            .ToDictionary(p => p.Name, p => p);

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
