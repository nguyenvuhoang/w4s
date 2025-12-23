using LinKit.Core.Cqrs;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

namespace O24OpenAPI.CTH.API.Application.Features.User;

public class UpdateUserBannerCommnad: BaseTransactionModel, ICommand<bool>
{
    public string UserCode { get; set; }
    public string Banner { get; set; }
}

public class UpdateUserBannerHandler(IUserBannerRepository userBannerRepository) : ICommandHandler<UpdateUserBannerCommnad, bool>
{
    public async Task<bool> HandleAsync(UpdateUserBannerCommnad request, CancellationToken cancellationToken = default)
    {
    var userBanner = userBannerRepository
            .Table.Where(x => x.UserCode == request.UserCode)
            .FirstOrDefault();
        if (userBanner != null)
        {
            userBanner.BannerSource = request.Banner;
            userBanner.UpdatedOnUTC = DateTime.UtcNow;
            await userBannerRepository.Update(userBanner);
        }
        else
        {
            userBanner = new UserBanner
            {
                UserCode = request.UserCode,
                BannerSource = request.Banner,
                CreatedOnUTC = DateTime.UtcNow,
            };
            await userBannerRepository.Insert(userBanner);
        }
        return true;
    }
}
