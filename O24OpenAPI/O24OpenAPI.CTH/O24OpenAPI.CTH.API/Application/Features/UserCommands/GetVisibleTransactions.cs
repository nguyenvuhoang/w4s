using LinKit.Core.Cqrs;
using LinqToDB;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.CTH.API.Application.Models;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Features.UserCommands;

public class GetVisibleTransactionsCommand
    : BaseTransactionModel,
        ICommand<List<VisibleTransactionResponse>>
{
    public string ChannelId { get; set; } = default!;
}

[CqrsHandler]
public class GetVisibleTransactionsHandle(IUserCommandRepository userCommandRepository)
    : ICommandHandler<GetVisibleTransactionsCommand, List<VisibleTransactionResponse>>
{
    [WorkflowStep(WorkflowStep.CTH.WF_STEP_CTH_GET_VISIBLE_TRANS)]
    public async Task<List<VisibleTransactionResponse>> HandleAsync(
        GetVisibleTransactionsCommand request,
        CancellationToken cancellationToken = default
    )
    {
        return await GetVisibleTransactions(request.ChannelId);
    }

    public async Task<List<VisibleTransactionResponse>> GetVisibleTransactions(string channelId)
    {
        var q =
            from userCommand in userCommandRepository.Table.Where(s =>
                s.ApplicationCode == channelId
                && s.Enabled
                && s.IsVisible
                && s.CommandType == "T"
                && s.ParentId != "0"
            )
            select new VisibleTransactionResponse
            {
                TransactionCode = userCommand.CommandId,
                TransactionName = userCommand.CommandName,
                TransactionNameLanguage = userCommand.CommandNameLanguage,
                ModuleCode = userCommand.ParentId,
            };
        return await q.ToListAsync();
    }
}
