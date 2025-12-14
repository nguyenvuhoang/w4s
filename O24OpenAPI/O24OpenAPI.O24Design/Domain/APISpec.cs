namespace O24OpenAPI.O24Design.Domain;

using O24OpenAPI.Core.Domain;

/// <summary>
/// Defines the <see cref="APISpec" />
/// </summary>
public class APISpec : BaseEntity
{
    /// <summary>
    /// Gets or sets the ApiServiceId
    /// </summary>
    public int ApiServiceId { get; set; }

    /// <summary>
    /// Gets or sets the Version
    /// </summary>
    public string Version { get; set; } = "v1";

    /// <summary>
    /// Gets or sets the SpecFormat
    /// </summary>
    public string SpecFormat { get; set; } = "OPENAPI";// OPENAPI, ASYNCAPI, PROTO

    /// <summary>
    /// Gets or sets the SpecRaw
    /// </summary>
    public string SpecRaw { get; set; } = default!;

    /// <summary>
    /// Gets or sets the Checksum
    /// </summary>
    public string? Checksum { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether IsActive
    /// </summary>
    public bool IsActive { get; set; } = true;
}
