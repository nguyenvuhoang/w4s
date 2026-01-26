using LinKit.Core.Cqrs;
using LinqToDB;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Core;
using O24OpenAPI.CTH.API.Application.Constants;
using O24OpenAPI.CTH.API.Application.Models;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Helpers;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Features.User;

public class GetUserByRoleIdCommand
    : BaseTransactionModel,
        ICommand<PagedListModel<UserAccount, UserAccountResponseModel>>
{
    public int RoleId { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

[CqrsHandler]
public class GetUserByRoleIdHandle(
    IUserAccountRepository userAccountRepository,
    IUserInRoleRepository userInRoleRepository
) : ICommandHandler<GetUserByRoleIdCommand, PagedListModel<UserAccount, UserAccountResponseModel>>
{
    [WorkflowStep(WorkflowStepCode.CTH.WF_STEP_CTH_GET_USER_BY_ROLE)]
    public async Task<PagedListModel<UserAccount, UserAccountResponseModel>> HandleAsync(
        GetUserByRoleIdCommand request,
        CancellationToken cancellationToken = default
    )
    {
        List<UserInRole> userList = await userInRoleRepository.GetUserInRolesByRoleIdAsync(
            request.RoleId
        );
        if (userList == null || userList.Count == 0)
        {
            throw await O24Exception.CreateAsync(
                O24CTHResourceCode.Validation.UserNotFoundByRoleId,
                request.Language,
                [request.RoleId.ToString()]
            );
        }

        List<string> userCodes = userList.Select(u => u.UserCode).ToList();

        List<UserAccount> users = await userAccountRepository
            .Table.Where(s => userCodes.Contains(s.UserCode))
            .ToListAsync();

        IPagedList<UserAccount> pageList = await users
            .AsQueryable()
            .ToPagedList(request.PageIndex, request.PageSize);
        return pageList.ToPagedListModel<UserAccount, UserAccountResponseModel>();
    }
}
