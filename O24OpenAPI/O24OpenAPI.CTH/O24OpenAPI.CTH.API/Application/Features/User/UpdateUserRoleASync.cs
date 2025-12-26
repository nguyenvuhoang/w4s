using LinKit.Core.Cqrs;
using LinqToDB;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Features.User;

public class UpdateUserRoleASyncCommnad : BaseTransactionModel, ICommand<bool>
{
    public int RoleId { get; set; }
    public List<string> ListOfUser { get; set; }
    public bool IsAssign { get; set; }
}

public class UpdateUserRoleASyncHandler(IUserInRoleRepository userInRoleRepository)
    : ICommandHandler<UpdateUserRoleASyncCommnad, bool>
{
    public async Task<bool> HandleAsync(
        UpdateUserRoleASyncCommnad request,
        CancellationToken cancellationToken = default
    )
    {
        if (request.IsAssign)
        {
            List<UserInRole> toInsert = new();

            foreach (var userCode in request.ListOfUser)
            {
                var exists = await userInRoleRepository
                    .Table.Where(u => u.UserCode == userCode && u.RoleId == request.RoleId)
                    .FirstOrDefaultAsync();

                if (exists == null)
                {
                    toInsert.Add(
                        new UserInRole
                        {
                            UserCode = userCode,
                            RoleId = request.RoleId,
                            IsMain = "Y",
                            CreatedOnUtc = DateTime.UtcNow,
                            UpdatedOnUtc = DateTime.UtcNow,
                        }
                    );
                }
            }

            if (toInsert.Count != 0)
            {
                await userInRoleRepository.BulkInsert(toInsert);
            }
        }
        else
        {
            // Bulk delete users from role
            var toDelete = await userInRoleRepository
                .Table.Where(u =>
                    request.ListOfUser.Contains(u.UserCode) && u.RoleId == request.RoleId
                )
                .ToListAsync();

            if (toDelete.Count != 0)
            {
                await userInRoleRepository.BulkDelete(toDelete);
            }
        }

        return true;
    }
}
