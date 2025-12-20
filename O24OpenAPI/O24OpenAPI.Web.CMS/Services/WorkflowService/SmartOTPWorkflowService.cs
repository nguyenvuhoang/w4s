using O24OpenAPI.Web.CMS.Models.Digital;
using O24OpenAPI.Web.CMS.Services.Interfaces;
using O24OpenAPI.Framework.Services;

namespace O24OpenAPI.Web.CMS.Services.WorkflowService;

/// <summary>
/// The authenticate workflow service class
/// </summary>
/// <seealso cref="BaseQueueService"/>

public class SmartOTPWorkflowService : BaseQueueService
{
    /// <summary>
    /// The authenticate service
    /// </summary>
    private readonly ISmartOTPService _smartOtp = EngineContext.Current.Resolve<ISmartOTPService>();


    /// <summary>
    /// ActiveOTP the jwt using the specified workflow
    /// </summary>
    /// <param name="workflow">The workflow</param>
    /// <returns>A task containing the workflow scheme</returns>
    public async Task<WorkflowScheme> ActiveOTP(WorkflowScheme workflow)
    {
        return await Invoke<BaseTransactionModel>(workflow, async () =>
        {
            var response = await _smartOtp.ActiveOTP(workflow);
            return response;
        });
    }

    /// <summary>
    /// ActiveOTP the jwt using the specified workflow
    /// </summary>
    /// <param name="workflow">The workflow</param>
    /// <returns>A task containing the workflow scheme</returns>
    public async Task<WorkflowScheme> RegisterOTP(WorkflowScheme workflow)
    {
        return await Invoke<BaseTransactionModel>(workflow, async () =>
        {
            var response = await _smartOtp.RegisterOTP(workflow);
            return response;
        });
    }

    /// <summary>
    /// VerifyOTP the hash password using the specified workflow
    /// </summary>
    /// <param name="workflow">The workflow</param>
    /// <returns>A task containing the workflow scheme</returns>
    public async Task<WorkflowScheme> VerifyOTP(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<VerifyOtpModel>();
        return await Invoke<VerifyOtpModel>(workflow, async () =>
        {
            var response = await _smartOtp.VerifyOTP(model);
            return response;
        });
    }
}
