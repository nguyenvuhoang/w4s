using System.Windows.Input;
using Newtonsoft.Json;

namespace O24OpenAPI.Core.Domain;

public abstract class BaseEntity
{
    [JsonProperty("id")]
    public int Id { get; set; }

    public DateTime? CreatedOnUtc { get; set; }
    public DateTime? UpdatedOnUtc { get; set; }

    [JsonIgnore]
    private readonly List<ICommand> _domainEvents = [];
    public IReadOnlyCollection<ICommand> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(ICommand domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public virtual List<AuditDiff>? GetChanges(BaseEntity oldEntity)
    {
        return null;
    }

    public virtual bool IsAuditable()
    {
        return false;
    }
}

public class AuditDiff
{
    public string? FieldName { get; set; }
    public object? OldValue { get; set; }
    public object? NewValue { get; set; }
    public decimal? Delta { get; set; } // For numeric fields = default(decimal?);
}
