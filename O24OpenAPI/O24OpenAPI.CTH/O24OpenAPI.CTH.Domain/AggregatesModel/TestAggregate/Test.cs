using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.CTH.Domain.AggregatesModel.TestAggregate;

[Auditable]
public partial class Test : BaseEntity
{
    public string? Name { get; set; }
    public decimal Amount { get; set; }

    public Test() { }

    public Test(string name, decimal amount)
    {
        Name = name;
        Amount = amount;
    }
}
