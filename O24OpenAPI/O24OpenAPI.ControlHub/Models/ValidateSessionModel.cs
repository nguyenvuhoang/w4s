using O24OpenAPI.Core.Abstractions;

namespace O24OpenAPI.ControlHub.Models;

/// <summary>
/// The validate session model class
/// </summary>
/// <seealso cref="BaseO24OpenAPIModel"/>
public class ValidateSessionModel : BaseO24OpenAPIModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ValidateSessionModel"/> class
    /// </summary>
    public ValidateSessionModel() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidateSessionModel"/> class
    /// </summary>
    /// <param name="errorCode">The error code</param>
    public ValidateSessionModel(string errorCode)
    {
        ErrorCode = errorCode;
    }

    /// <summary>
    /// Gets or sets the value of the is valid
    /// </summary>
    public bool IsValid { get; set; } = true;

    /// <summary>
    /// Gets or sets the value of the ip address
    /// </summary>
    public string IpAddress { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the value of the device name
    /// </summary>
    public string DeviceName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the value of the error msg
    /// </summary>
    public string ErrorMsg { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the value of the error code
    /// </summary>
    public string ErrorCode { get; set; } = string.Empty;

    public ValidateSessionModel(bool isValidSession, string deviceName)
    {
        IsValid = isValidSession;
        DeviceName = deviceName;
    }
}
