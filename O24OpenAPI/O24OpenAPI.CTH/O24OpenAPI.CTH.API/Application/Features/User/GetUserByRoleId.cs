using System.Windows.Input;
using LinKit.Core.Cqrs;
using LinqToDB;
using O24OpenAPI.Client.Scheme.Workflow;
using O24OpenAPI.Core;
using O24OpenAPI.CTH.API.Application.Constants;
using O24OpenAPI.CTH.API.Application.Models;
using O24OpenAPI.CTH.API.Application.Models.Roles;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Helpers;
using O24OpenAPI.Framework.Infrastructure.Mapper.Extensions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.Framework.Services.Queue;

namespace O24OpenAPI.CTH.API.Application.Features.User
{
    public class GetUserByRoleIdCommand
        : BaseTransactionModel,
            ICommand<PagedListModel<UserAccount, UserAccountResponseModel>>
    {
        public int RoleId { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? Language { get; set; }
    }

    [CqrsHandler]
    public class GetUserByRoleIdHandle(
        IUserAccountRepository userAccountRepository,
        IUserInRoleRepository userInRoleRepository
    )
        : ICommandHandler<
            GetUserByRoleIdCommand,
            PagedListModel<UserAccount, UserAccountResponseModel>
        >
    {
        [WorkflowStep("WF_STEP_BO_GET_USER_BY_ROLE")]
        public async Task<PagedListModel<UserAccount, UserAccountResponseModel>> HandleAsync(
            GetUserByRoleIdCommand request,
            CancellationToken cancellationToken = default
        )
        {
            var model = new ModelWithRoleId
            {
                RoleId = request.RoleId,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Language = request.Language,
            };

            var response = await GetUserByRoleIdASync(model);
            return response.ToPagedListModel<UserAccount, UserAccountResponseModel>();
        }

        public async Task<IPagedList<UserAccount>> GetUserByRoleIdASync(ModelWithRoleId model)
        {
            var userList = await userInRoleRepository.GetUserInRolesByRoleIdAsync(model.RoleId);
            if (userList == null || userList.Count == 0)
            {
                throw await O24Exception.CreateAsync(
                    O24CTHResourceCode.Validation.UserNotFoundByRoleId,
                    model.Language,
                    [model.RoleId.ToString()]
                );
            }

            var userCodes = userList.Select(u => u.UserCode).ToList();

            var users = await userAccountRepository
                .Table.Where(s => userCodes.Contains(s.UserCode))
                .ToListAsync();

            var pageList = await users.AsQueryable().ToPagedList(model.PageIndex, model.PageSize);
            return pageList;
        }
    }
}
