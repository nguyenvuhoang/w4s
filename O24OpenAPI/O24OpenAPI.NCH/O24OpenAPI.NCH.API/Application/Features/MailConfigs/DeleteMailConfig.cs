using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.NCH.Domain.AggregatesModel.MailAggregate;

namespace O24OpenAPI.NCH.API.Application.Features.MailConfigs;

public class DeleteMailConfigCommand : BaseTransactionModel, ICommand<bool>
{
    public int Id { get; set; }
}

[CqrsHandler]
public class DeleteMailConfigHandler(IMailConfigRepository mailConfigRepository)
    : ICommandHandler<DeleteMailConfigCommand, bool>
{
    [WorkflowStep(WorkflowStepCode.NCH.WF_STEP_NCH_DELETE_MAIL_CONFIG)]
    public async Task<bool> HandleAsync(
        DeleteMailConfigCommand request,
        CancellationToken cancellationToken = default
    )
    {
        MailConfig mailConfig = await mailConfigRepository.GetById(request.Id);
        await mailConfigRepository.Delete(mailConfig);
        return true;
    }
}
