//using LinKit.Core.Cqrs;
//using LinqToDB;
//using O24OpenAPI.CTH.API.Application.Constants;
//using O24OpenAPI.CTH.Constant;
//using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
//using O24OpenAPI.Framework.Models;

//namespace O24OpenAPI.CTH.API.Application.Features.User
//{
//    public class CreateUserRoleCommnad : BaseTransactionModel, ICommand<bool>
//    {
//        public string RoleName { get; set; }
//        public string RoleType { get; set; }
//        public string ServiceID { get; set; }
//        public string RoleDescription { get; set; }
//    }

//    public class CreateUserRoleHandler(IUserRoleRepository userRoleRepository, IUserCommandRepository userCommandRepository, IUserRightRepository userRightRepository) : ICommandHandler<CreateUserRoleCommnad, bool>
//    {
//        public async Task<bool> HandleAsync(CreateUserRoleCommnad request, CancellationToken cancellationToken = default)
//        {
//            var roleId = await userRoleRepository.GetNextRoleIdAsync();

//            var newUserRole = new UserRole
//            {
//                RoleId = roleId,
//                RoleName = request.RoleName,
//                RoleDescription = request.RoleDescription,
//                UserType = Code.UserType.BO,
//                UserCreated = request.CurrentUserCode,
//                DateCreated = DateTime.UtcNow,
//                ServiceID = Code.UserType.BO,
//                RoleType = request.RoleType,
//                Status = Code.ShowStatus.ACTIVE,
//                IsShow = Code.ShowStatus.YES,
//                SortOrder = roleId,
//            };

//            var inserted = await userRoleRepository.InsertAsync(newUserRole);
//            if (inserted == null)
//            {
//                return false;
//            }

//            var parentCommandIds = await userCommandRepository.GetListCommandParentAsync(request.ChannelId);
//            if (parentCommandIds == null || parentCommandIds.Count == 0)
//            {
//                return true;
//            }

//            var existedCmdIds = await userRightRepository
//                .Table.Where(r => r.RoleId == roleId && parentCommandIds.Contains(r.CommandId))
//                .Select(r => r.CommandId)
//                .Distinct()
//                .ToListAsync();

//            var now = DateTime.UtcNow;
//            var toInsert = parentCommandIds
//                .Except(existedCmdIds)
//                .Distinct()
//                .Select(pid => new UserRight
//                {
//                    RoleId = roleId,
//                    CommandId = pid,
//                    CommandIdDetail = Common.ACTIVE,
//                    Invoke = 1,
//                    Approve = 1,
//                    CreatedOnUtc = now,
//                    UpdatedOnUtc = now,
//                })
//                .ToList();

//            if (toInsert.Count > 0)
//            {
//                await userRightRepository.BulkInsert(toInsert);
//            }

//            return true;
//        }
//    }
//}
