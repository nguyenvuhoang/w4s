using O24OpenAPI.Core;
using O24OpenAPI.Data;
using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.O24NCH.Domain;
using O24OpenAPI.O24NCH.Models.Request;
using O24OpenAPI.O24NCH.Services.Interfaces;
using O24OpenAPI.Web.Framework.Localization;
using O24OpenAPI.Web.Framework.Models;

namespace O24OpenAPI.O24NCH.Services.Services;

/// <summary>
/// MailConfig service
/// </summary>
public partial class MailConfigService : IMailConfigService
{
    #region Fields

    private readonly ILocalizationService _localizationService;

    private readonly IRepository<MailConfig> _mailConfigRepository;
    private readonly StringComparison ICIC = StringComparison.InvariantCultureIgnoreCase;
    private readonly ISendMailService _sendMailService;

    #endregion

    #region Ctor
    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="localizationService"></param>
    /// <param name="MailConfigRepository"></param>
    /// <param name="sendMailService"></param>
    public MailConfigService(
        ILocalizationService localizationService,
        IRepository<MailConfig> MailConfigRepository,
        ISendMailService sendMailService
    )
    {
        _localizationService = localizationService;
        _mailConfigRepository = MailConfigRepository;
        _sendMailService = sendMailService;
    }

    #endregion
    /// <summary>
    /// Gets GetById
    /// </summary>
    /// <returns>Task&lt;MailConfig&gt;.</returns>
    public virtual async Task<MailConfig> GetById(int id)
    {
        return await _mailConfigRepository.GetById(id);
    }

    /// <summary>
    /// Gets GetByTxcodeAndApp
    /// </summary>
    /// <returns>Task&lt;MailConfigModel&gt;.</returns>
    public virtual async Task<IPagedList<MailConfig>> Search(SimpleSearchModel model)
    {
        return await _mailConfigRepository.GetAllPaged(
            query =>
            {
                if (!string.IsNullOrEmpty(model.SearchText))
                {
                    query = query.Where(c =>
                        c.Host.Contains(model.SearchText)
                        || c.Port.ToString().Contains(model.SearchText)
                        || c.Sender.Contains(model.SearchText)
                    );
                }

                query = query.OrderBy(c => c.Id);
                return query;
            },
            0,
            0
        );
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="mailConfig"></param>
    /// <returns></returns>
    public virtual async Task<MailConfig> Create(MailConfig mailConfig)
    {
        if (!_sendMailService.IsValidEmail(mailConfig.Sender))
        {
            throw new O24OpenAPIException(
                await _localizationService.GetResource("Email.InvalidEmail")
            );
        }

        if (
            !string.IsNullOrEmpty(mailConfig.EmailTest)
            && !_sendMailService.IsValidEmail(mailConfig.EmailTest)
        )
        {
            throw new O24OpenAPIException(
                await _localizationService.GetResource("Email.InvalidEmail")
            );
        }

        if (await ValidMailConfig(mailConfig.ConfigId))
        {
            throw new O24OpenAPIException(
                await _localizationService.GetResource("Email.InvalidMailConfig")
            );
        }

        await _mailConfigRepository.Insert(mailConfig);
        return mailConfig;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="mailConfig"></param>
    /// <returns></returns>
    public virtual async Task<MailConfigUpdateModel> Update(
        MailConfigUpdateModel mailConfig,
        string referenceId = ""
    )
    {
        if (!_sendMailService.IsValidEmail(mailConfig.Sender))
        {
            throw new O24OpenAPIException(
                await _localizationService.GetResource("Email.InvalidEmail")
            );
        }

        if (
            !string.IsNullOrEmpty(mailConfig.EmailTest)
            && !_sendMailService.IsValidEmail(mailConfig.EmailTest)
        )
        {
            throw new O24OpenAPIException(
                await _localizationService.GetResource("Email.InvalidEmail")
            );
        }

        var getMailConfig =
            await GetById(mailConfig.Id) ?? throw new O24OpenAPIException("MailConfig not found");
        getMailConfig.Sender = mailConfig.Sender;
        getMailConfig.Host = mailConfig.Host;
        getMailConfig.Port = mailConfig.Port;
        getMailConfig.EnableTLS = mailConfig.EnableTLS;
        getMailConfig.EmailTest = mailConfig.EmailTest;
        getMailConfig.Password = mailConfig.Password;

        await _mailConfigRepository.Update(getMailConfig, referenceId);
        return mailConfig;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="mailConfig"></param>
    /// <returns></returns>
    public virtual async Task<MailConfig> Delete(MailConfig mailConfig)
    {
        await _mailConfigRepository.Delete(mailConfig);
        return mailConfig;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="configId"></param>
    /// <returns></returns>
    public virtual async Task<MailConfig> GetByConfigId(string configId)
    {
        return await _mailConfigRepository
            .Table.Where(s => s.ConfigId.Equals(configId))
            .FirstOrDefaultAsync();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<IPagedList<MailConfig>> Search(MailConfigSearchModel model)
    {
        model.PageSize = model.PageSize == 0 ? int.MaxValue : model.PageSize;
        var query = _mailConfigRepository.Table;
        if (!string.IsNullOrEmpty(model.ConfigId))
        {
            query = query.Where(c => c.ConfigId.Contains(model.ConfigId, ICIC));
        }

        if (!string.IsNullOrEmpty(model.Host))
        {
            query = query.Where(c => c.Host.Contains(model.Host, ICIC));
        }

        if (!string.IsNullOrEmpty(model.Sender))
        {
            query = query.Where(c => c.Sender.Contains(model.Sender, ICIC));
        }

        if (!string.IsNullOrEmpty(model.Password))
        {
            query = query.Where(c => c.Password.Contains(model.Password, ICIC));
        }

        if (!string.IsNullOrEmpty(model.EmailTest))
        {
            query = query.Where(c => c.EmailTest.Contains(model.EmailTest, ICIC));
        }

        if (model.EnableTLS)
        {
            query = query.Where(c => c.EnableTLS);
        }

        query = query.OrderBy(c => c.ConfigId);

        var response = await query.ToPagedList(model.PageIndex, model.PageSize);
        return response;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="configId"></param>
    /// <returns></returns>
    public async Task<bool> ValidMailConfig(string configId)
    {
        var result = false;
        var query = await _mailConfigRepository
            .Table.Where(s => s.ConfigId == configId)
            .FirstOrDefaultAsync();
        if (query != null)
        {
            result = true;
        }

        return result;
    }
}
