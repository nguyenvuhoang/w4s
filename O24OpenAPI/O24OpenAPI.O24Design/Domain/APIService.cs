namespace O24OpenAPI.O24Design.Domain;

using O24OpenAPI.Core.Domain;

/// <summary>
/// Defines the <see cref="APIService" />
/// </summary>
public class APIService : BaseEntity
{
    /// <summary>
    /// Gets or sets the ServiceCode
    /// </summary>
    public string ServiceCode { get; set; } = default!;

    /// <summary>
    /// Gets or sets the DisplayName
    /// </summary>
    public string DisplayName { get; set; } = default!;

    /// <summary>
    /// Gets or sets the Description
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the ServiceType
    /// </summary>
    public string ServiceType { get; set; } = "REST";// REST, GRPC, ASYNC, MCP

    /// <summary>
    /// Gets or sets the BaseUrl
    /// </summary>
    public string BaseUrl { get; set; } = default!;

    /// <summary>
    /// Gets or sets the OwnerTeam
    /// </summary>
    public string OwnerTeam { get; set; }

    /// <summary>
    /// Gets or sets the ContactEmail
    /// </summary>
    public string ContactEmail { get; set; }

    /// <summary>
    /// Gets or sets the Tags
    /// </summary>
    public string Tags { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether IsActive
    /// </summary>
    public bool IsActive { get; set; } = true;
}
