using Newtonsoft.Json;
using System.Windows.Input;

namespace O24OpenAPI.Core.Domain;

/// <summary>
/// The base entity class
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Gets or sets the value of the id
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    public DateTime? CreatedOnUtc { get; set; }
    public DateTime? UpdatedOnUtc { get; set; }

    private readonly List<ICommand> _domainEvents = [];
    public IReadOnlyCollection<ICommand> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(ICommand domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}
