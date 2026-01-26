using LinKit.Core.Cqrs;
using LinqToDB;
using O24OpenAPI.APIContracts.Models.CTH;
using O24OpenAPI.Core;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.Logging.Helpers;

namespace O24OpenAPI.CTH.API.Application.Features.UserCommands;

public class LoadFullUserCommandsQuery
    : SimpleSearchModel,
        IQuery<IPagedList<CTHUserCommandModel>>
{
    public string CommandId { get; set; } = string.Empty;
    public string CommandName { get; set; } = string.Empty;
    public string ApplicationCode { get; set; } = string.Empty;
    public string CommandType { get; set; } = string.Empty;
    public bool? IsVisible { get; set; }

}

[CqrsHandler]
public class LoadFullUserCommandsQueryHandler(IUserCommandRepository _userCommandRepository)
    : IQueryHandler<LoadFullUserCommandsQuery, IPagedList<CTHUserCommandModel>>
{
    public async Task<IPagedList<CTHUserCommandModel>> HandleAsync(
        LoadFullUserCommandsQuery query,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var q = _userCommandRepository.Table.AsQueryable();
            if (!string.IsNullOrWhiteSpace(query.CommandId))
            {
                q = q.Where(x => x.CommandId == query.CommandId);
            }

            if (!string.IsNullOrWhiteSpace(query.CommandName))
            {
                q = q.Where(x => x.CommandName == query.CommandName);
            }

            if (!string.IsNullOrWhiteSpace(query.ApplicationCode))
            {
                q = q.Where(x => x.ApplicationCode == query.ApplicationCode);
            }

            if (!string.IsNullOrWhiteSpace(query.CommandType))
            {
                q = q.Where(x => x.CommandType == query.CommandType);
            }

            if (query.IsVisible.HasValue)
            {
                q = q.Where(x => x.IsVisible == query.IsVisible.Value);
            }


            q = q
            .OrderBy(x => x.ApplicationCode)
            .ThenBy(x => x.CommandId)
            .ThenBy(x => x.DisplayOrder);

            var result = q.Select(x => new CTHUserCommandModel
            {
                ApplicationCode = x.ApplicationCode,
                CommandId = x.CommandId,
                ParentId = x.ParentId,
                CommandName = x.CommandName,
                CommandNameLanguage = x.CommandNameLanguage,
                CommandType = x.CommandType,
                CommandURI = x.CommandURI ?? "",
                Enabled = x.Enabled,
                IsVisible = x.IsVisible,
                DisplayOrder = x.DisplayOrder,
                GroupMenuIcon = x.GroupMenuIcon,
                GroupMenuVisible = x.GroupMenuVisible,
                GroupMenuId = x.GroupMenuId ?? "",
                GroupMenuListAuthorizeForm = x.GroupMenuListAuthorizeForm ?? "",
            });

            return await result.ToPagedList(query.PageIndex, query.PageSize);

        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync();
            BusinessLogHelper.Error(ex, "Error while loading full user commands");
            return default;
        }
    }
}
