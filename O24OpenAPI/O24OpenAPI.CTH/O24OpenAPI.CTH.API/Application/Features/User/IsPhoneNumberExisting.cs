using LinKit.Core.Cqrs;
using LinqToDB;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.CTH.API.Application.Constants;
using O24OpenAPI.CTH.API.Application.Models;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Features.User;

public class IsPhoneNumberExistingCommand : BaseTransactionModel, ICommand<bool>
{
    public UserAccountPhoneModel Model { get; set; } = default!;
}

[CqrsHandler]
public class IsPhoneNumberExistingHandle(IUserAccountRepository userAccountRepository)
    : ICommandHandler<IsPhoneNumberExistingCommand, bool>
{
    [WorkflowStep(WorkflowStep.CTH.WF_STEP_CTH_CHECK_USER_PHONE_NUMBER)]
    public async Task<bool> HandleAsync(
        IsPhoneNumberExistingCommand request,
        CancellationToken cancellationToken = default
    )
    {
        return await IsPhoneNumberExistingAsync(request.Model);
    }

    public async Task<bool> IsPhoneNumberExistingAsync(UserAccountPhoneModel model)
    {
        var existingPhonenumber = await userAccountRepository
            .Table.Where(c => c.Status != "D")
            .FirstOrDefaultAsync(c => c.Phone == model.PhoneNumber.Trim());

        if (existingPhonenumber != null)
        {
            throw await O24Exception.CreateAsync(
                O24CTHResourceCode.Validation.PhoneNumberIsExisting,
                model.Language,
                [model.PhoneNumber]
            );
        }
        return true;
    }
}
