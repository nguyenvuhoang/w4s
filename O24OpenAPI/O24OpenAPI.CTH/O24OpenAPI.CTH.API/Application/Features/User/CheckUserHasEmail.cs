using LinKit.Core.Cqrs;
using LinqToDB;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Core;
using O24OpenAPI.CTH.API.Application.Models;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Infrastructure.Mapper.Extensions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Features.User;

public class CheckUserHasEmailCommand : BaseTransactionModel, ICommand<string>
{
    public string UserCode { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string DeviceId { get; set; }
}

[CqrsHandler]
public class CheckUserHasEmailHandle(IUserAccountRepository userAccountRepository)
    : ICommandHandler<CheckUserHasEmailCommand, string>
{
    [WorkflowStep(WorkflowStepCode.CTH.WF_STEP_CTH_CHECK_EMAIL)]
    public async Task<string> HandleAsync(
        CheckUserHasEmailCommand request,
        CancellationToken cancellationToken = default
    )
    {
        var model = request.ToModel<DefaultModel>();
        return await CheckUserHasEmail(model);
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
