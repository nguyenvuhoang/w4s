using LinKit.Core.Cqrs;
using O24OpenAPI.Core.Constants;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Features.User;

public class UpdateUserAvatarCommnad : BaseTransactionModel, ICommand<bool>
{
    public string UserCode { get; set; }
    public string AvatarUrl { get; set; }
    public DateTime DateInsert { get; set; }
}

[CqrsHandler]
public class UpdateUserAvatarHandler(IUserAvatarRepository userAvatarRepository)
    : ICommandHandler<UpdateUserAvatarCommnad, bool>
{
    [WorkflowStep("WF_STEP_CTH_UPDATE_USER")]
    public async Task<bool> HandleAsync(
        UpdateUserAvatarCommnad request,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var entity = await userAvatarRepository.GetByUserCodeAsync(request.UserCode);

            if (entity == null)
            {
                entity = new UserAvatar
                {
                    UserCode = request.UserCode,
                    ImageUrl = request.AvatarUrl,
                    DateInsert = DateTime.UtcNow,
                };

                await userAvatarRepository.InsertAsync(entity);
            }
            else
            {
                entity.ImageUrl = request.AvatarUrl;
                await userAvatarRepository.Update(entity);
            }

            return true;
        }
        catch (O24Exception)
        {
            throw;
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync(
                $"[UpdateUserAvatarAsync] Error for {request.UserCode}: {ex.Message}"
            );
            throw await O24Exception.CreateAsync(
                ResourceCode.Common.SystemError,
                request.Language,
                ex
            );
        }
    }
}
