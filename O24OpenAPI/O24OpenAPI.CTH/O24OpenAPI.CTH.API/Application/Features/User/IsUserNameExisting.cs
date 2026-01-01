using LinKit.Core.Cqrs;
using LinqToDB;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.CTH.API.Application.Constants;
using O24OpenAPI.CTH.API.Application.Models;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Infrastructure.Mapper.Extensions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Features.User;

public class IsUserNameExistingCommand : BaseTransactionModel, ICommand<bool>
{
    public string UserCode { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string DeviceId { get; set; }
}

[CqrsHandler]
public class IsUserNameExistingHandle(IUserAccountRepository userAccountRepository)
    : ICommandHandler<IsUserNameExistingCommand, bool>
{
    [WorkflowStep(WorkflowStep.CTH.WF_STEP_CTH_VALIDATE_USER)]
    public async Task<bool> HandleAsync(
        IsUserNameExistingCommand request,
        CancellationToken cancellationToken = default
    )
    {
        var model = request.ToModel<DefaultModel>();
        return await IsUserNameExistingAsync(model);
    }

    public async Task<bool> IsUserNameExistingAsync(DefaultModel model)
    {
        var existingUserName = await userAccountRepository.Table.FirstOrDefaultAsync(c =>
            c.UserName == model.UserName.Trim() && c.UserCode == model.UserCode.Trim()
        );

        if (existingUserName != null)
        {
            throw await O24Exception.CreateAsync(
                O24CTHResourceCode.Validation.UsernameIsNotExist,
                model.Language,
                [model.UserName]
            );
        }
        return true;
    }
}
