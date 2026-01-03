using LinKit.Core.Cqrs;
using LinqToDB;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.CTH.API.Application.Models.Roles;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Features.User;

public class LoadMenuByChannelCommand
    : BaseTransactionModel,
        ICommand<List<UserCommandResponseModel>>
{
    public string CommandId { get; set; }
    public string Channel { get; set; } = string.Empty;
}

[CqrsHandler]
public class LoadMenuByChannelHandle(IUserCommandRepository userCommandRepository)
    : ICommandHandler<LoadMenuByChannelCommand, List<UserCommandResponseModel>>
{
    [WorkflowStep(WorkflowStepCode.CTH.WF_STEP_CTH_LOAD_MENU)]
    public async Task<List<UserCommandResponseModel>> HandleAsync(
        LoadMenuByChannelCommand request,
        CancellationToken cancellationToken = default
    )
    {
        var commandMenus = await userCommandRepository
            .Table.Where(s =>
                s.ApplicationCode == request.ChannelId && s.CommandType == "M" && s.Enabled
            )
            .Select(s => new UserCommandResponseModel(s))
            .ToListAsync();

        return [.. commandMenus.OrderBy(x => x.DisplayOrder)];
    }
}
