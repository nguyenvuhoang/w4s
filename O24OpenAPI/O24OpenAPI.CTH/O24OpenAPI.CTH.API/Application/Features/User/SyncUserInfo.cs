using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.CTH.API.Application.Models;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Infrastructure.Mapper.Extensions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Features.User;

public class SyncUserInfoCommand : BaseTransactionModel, ICommand<bool>
{
    public string ContractNumber { get; set; }
    public string PhoneNumber { get; set; }
}

[CqrsHandler]
public class SyncUserInfoHandle(IUserAccountRepository userAccountRepository)
    : ICommandHandler<SyncUserInfoCommand, bool>
{
    [WorkflowStep(WorkflowStep.CTH.WF_STEP_CTH_SYNC_USER_INFO)]
    public async Task<bool> HandleAsync(
        SyncUserInfoCommand request,
        CancellationToken cancellationToken = default
    )
    {
        var model = request.ToModel<SyncUserInfoModel>();
        return await SyncUserInfoAsync(model);
    }

    public async Task<bool> SyncUserInfoAsync(SyncUserInfoModel model)
    {
        if (model == null)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(model.ContractNumber))
        {
            return false;
        }

        var user = await userAccountRepository.GetUserByContractNumber(model.ContractNumber.Trim());
        if (user == null)
        {
            return false;
        }

        var newPhone = NormalizePhone(model.PhoneNumber);
        if (string.IsNullOrWhiteSpace(newPhone))
        {
            return false;
        }

        if (string.Equals(user.Phone, newPhone, StringComparison.Ordinal))
        {
            return true;
        }

        user.Phone = newPhone;
        user.UpdatedOnUtc = DateTime.UtcNow;
        user.UserModified = model.CurrentUserCode ?? "SYSTEM";

        await userAccountRepository.Update(user);
        return true;
    }

    private static string NormalizePhone(string? input)
    {
        var v = (input ?? string.Empty).Trim();
        if (v.Length == 0)
        {
            return string.Empty;
        }

        v = v.Replace(" ", "").Replace("-", "").Replace(".", "").Replace("(", "").Replace(")", "");

        return v;
    }
}
