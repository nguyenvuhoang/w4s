using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Models;

public class UpdateUserRequestModel : BaseTransactionModel
{
    public UpdateUserRequestModel() { }

    public int Id { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Phone { get; set; }
    public int? Gender { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public new string Status { get; set; } = "A";

    public List<string> ChangedFields { get; set; } = [];

    public static UpdateUserRequestModel FromUpdatedEntity(
        UserAccount updated,
        UserAccount original
    )
    {
        var result = new UpdateUserRequestModel();
        var entityProps = typeof(UserAccount).GetProperties();
        var modelProps = typeof(UpdateUserRequestModel).GetProperties().ToDictionary(p => p.Name);

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
