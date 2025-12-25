using LinKit.Core.Cqrs;
using LinqToDB;
using O24OpenAPI.Client.Scheme.Workflow;
using O24OpenAPI.Core;
using O24OpenAPI.CTH.API.Application.Features.User;
using O24OpenAPI.CTH.API.Application.Models;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Features.User
{
    public class CheckUserHasEmailCommand : BaseTransactionModel, ICommand<string>
    {
        public DefaultModel Model { get; set; } = default!;
    }

    [CqrsHandler]
    public class CheckUserHasEmailHandle(IUserAccountRepository userAccountRepository)
        : ICommandHandler<CheckUserHasEmailCommand, string>
    {
        [WorkflowStep("WF_STEP_CTH_CHECK_EMAIL")]
        public async Task<string> HandleAsync(
            CheckUserHasEmailCommand request,
            CancellationToken cancellationToken = default
        )
        {
            return await CheckUserHasEmail(request.Model);
        }

        public async Task<string> CheckUserHasEmail(DefaultModel model)
        {
            var user = await userAccountRepository
                .Table.Where(s => s.UserCode == model.UserCode)
                .FirstOrDefaultAsync();
            return string.IsNullOrWhiteSpace(user?.Email)
                ? throw new O24OpenAPIException("This user does not have an email!")
                : user.Email;
        }
    }
}
