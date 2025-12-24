using LinKit.Core.Cqrs;
using O24OpenAPI.Client.Scheme.Workflow;
using O24OpenAPI.CTH.API.Application.Constants;
using O24OpenAPI.CTH.API.Application.Models;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Features.User
{
    public class IsLoginCommand : BaseTransactionModel, ICommand<bool>
    {
        public DefaultModel Model { get; set; } = default!;
    }

    [CqrsHandler]
    public class IsLoginHandle(IUserAccountRepository userAccountRepository)
        : ICommandHandler<IsLoginCommand, bool>
    {
        [WorkflowStep("WF_STEP_CTH_GET_USER_STATUS_LOGIN")]
        public async Task<bool> HandleAsync(
            IsLoginCommand request,
            CancellationToken cancellationToken = default
        )
        {
            return await IsLoginAsync(request.Model);
        }

        public async Task<bool> IsLoginAsync(DefaultModel model)
        {
            var userAccount =
                await userAccountRepository.GetByUserCodeAsync(model.UserCode)
                ?? throw await O24Exception.CreateAsync(
                    O24CTHResourceCode.Validation.UsernameIsNotExist,
                    model.Language,
                    [model.UserCode]
                );

            return userAccount.IsLogin ?? false;
        }
    }
}
