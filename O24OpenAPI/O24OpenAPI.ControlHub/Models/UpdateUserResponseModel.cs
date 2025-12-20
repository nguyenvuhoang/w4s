using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.ControlHub.Models;

public class UpdateUserResponseModel : BaseO24OpenAPIModel
{
    public UpdateUserResponseModel() { }

    public int Id { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Phone { get; set; }
    public int Gender { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public List<string> ChangedFields { get; set; } = [];

    public static UpdateUserResponseModel FromUpdatedEntity(
        UserAccount updated,
        UserAccount original
    )
    {
        var result = new UpdateUserResponseModel();
        var entityProps = typeof(UserAccount).GetProperties();
        var modelProps = typeof(UpdateUserResponseModel).GetProperties().ToDictionary(p => p.Name);

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
