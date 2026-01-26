using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Core;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Features.UserCommands;

public class DeleteMenuCommand
    : BaseTransactionModel,
        ICommand<bool>
{
    public string ApplicationCode { get; set; } = string.Empty;
    public string CommandId { get; set; } = string.Empty;

}

[CqrsHandler]
public class DeleteMenuCommandHandler(IUserCommandRepository userCommandRepository)
    : ICommandHandler<DeleteMenuCommand, bool>
{
    [WorkflowStep(WorkflowStepCode.CTH.WF_STEP_CTH_DELETE_MENU)]
    public async Task<bool> HandleAsync(
        DeleteMenuCommand request,
        CancellationToken cancellationToken = default
    )
    {

        var userCommandExists = await userCommandRepository.GetByCommandIdAsync(request.CommandId, request.ApplicationCode) ??
            throw new O24OpenAPIException($"CommandId {request.CommandId} is does not exists.");

        await userCommandRepository.DeleteAsync(userCommandExists);
        return true;
    }
}
