using Newtonsoft.Json.Linq;
using O24OpenAPI.Web.CMS.Models.Digital;

namespace O24OpenAPI.Web.CMS.Services.Interfaces;

/// <summary>
/// The smart otp service interface
/// </summary>

public interface ISmartOTPService
{
    /// <summary>
    /// /// Registers the otp using the specified workflow
    /// </summary>
    /// <param name="workflow">The workflow</param>
    /// <returns>A task containing the token</returns>
    Task<JToken> RegisterOTP(WorkflowScheme workflow);

    /// <summary>
    /// Actives the otp using the specified workflow
    /// </summary>
    /// <param name="workflow">The workflow</param>
    /// <returns>A task containing the token</returns>
    Task<JToken> ActiveOTP(WorkflowScheme workflow);

    /// <summary>
    /// Verifies the otp using the specified workflow
    /// </summary>
    /// <param name="workflow">The workflow</param>
    /// <returns>A task containing the token</returns>
    Task<JToken> VerifyOTP(VerifyOtpModel model);
}
