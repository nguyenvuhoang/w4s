using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Features.Signature;

public class SignatureRequestModel : BaseTransactionModel, ICommand<bool>
{
    public string PublicKey { get; set; }
}

[CqrsHandler]
public class SavePublicKeyHandler(IUserSessionRepository userSessionRepository)
    : ICommandHandler<SignatureRequestModel, bool>
{
    [WorkflowStep(WorkflowStepCode.CTH.WF_STEP_CTH_REGISTER_SIGNATURE_KEY)]
    public async Task<bool> HandleAsync(
        SignatureRequestModel request,
        CancellationToken cancellationToken = default
    )
    {
        if (string.IsNullOrEmpty(request.PublicKey))
        {
            throw new Exception("Public key is empty");
        }
        await userSessionRepository.UpdateSignatureKey(
            token: request.Token,
            signatureKey: request.PublicKey
        );
        return true;
    }
}
