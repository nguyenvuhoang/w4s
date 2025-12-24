using LinKit.Core.Cqrs;
using LinqToDB;
using O24OpenAPI.Client.Scheme.Workflow;
using O24OpenAPI.CTH.API.Application.Models.Roles;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.CTH.Infrastructure.Repositories;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Infrastructure.Mapper.Extensions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Features.User
{
    public class LoadMenuByChannelCommand
        : BaseTransactionModel,
            ICommand<List<UserCommandResponseModel>> { }

    [CqrsHandler]
    public class LoadMenuByChannelHandle(
        WFScheme wFScheme,
        IUserCommandRepository userCommandRepository
    ) : ICommandHandler<LoadMenuByChannelCommand, List<UserCommandResponseModel>>
    {
        [WorkflowStep("WF_STEP_CTH_LOAD_MENU")]
        public async Task<List<UserCommandResponseModel>> HandleAsync(
            LoadMenuByChannelCommand request,
            CancellationToken cancellationToken = default
        )
        {
            var model = wFScheme.ToModel<UserCommandRequestModel>();

            var response = await LoadMenuByChannelAsync(model);
            return response;
        }

        public async Task<List<UserCommandResponseModel>> LoadMenuByChannelAsync(
            UserCommandRequestModel model
        )
        {
            var commandMenus = await GetCommandMenuByChannel(model.ChannelId);

            return [.. commandMenus.OrderBy(x => x.DisplayOrder)];
        }

        public async Task<List<UserCommandResponseModel>> GetCommandMenuByChannel(string channelId)
        {
            return await userCommandRepository
                .Table.Where(s =>
                    s.ApplicationCode == channelId && s.CommandType == "M" && s.Enabled
                )
                .Select(s => new UserCommandResponseModel(s))
                .ToListAsync();
        }
    }
}
