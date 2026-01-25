using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Attributes;
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
    [WorkflowStep(WorkflowStepCode.CTH.WF_STEP_CTH_SYNC_USER_INFO)]
    public async Task<bool> HandleAsync(
        SyncUserInfoCommand request,
        CancellationToken cancellationToken = default
    )
    {
        if (request == null)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.ContractNumber))
        {
            return false;
        }

        UserAccount user = await userAccountRepository.GetUserByContractNumber(
            request.ContractNumber.Trim()
        );
        if (user == null)
        {
            return false;
        }

        string newPhone = NormalizePhone(request.PhoneNumber);
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
        user.UserModified = request.CurrentUserCode ?? "SYSTEM";

        await userAccountRepository.Update(user);
        return true;
    }

    private static string NormalizePhone(string input)
    {
        string v = (input ?? string.Empty).Trim();
        if (v.Length == 0)
        {
            return string.Empty;
        }

        v = v.Replace(" ", "").Replace("-", "").Replace(".", "").Replace("(", "").Replace(")", "");

        return v;
    }
}
