using O24OpenAPI.CMS.API.Application.Services.Interfaces;
using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.Framework.Localization;

namespace O24OpenAPI.CMS.API.Application.Services.Services;

/// <summary>
/// MailTemplate service
/// </summary>
/// <remarks>
/// Ctor
/// </remarks>
/// <param name="localizationService"></param>
/// <param name="MailTemplateRepository"></param>
public partial class MailTemplateService(
    ILocalizationService localizationService,
    IRepository<MailTemplate> MailTemplateRepository
) : IMailTemplateService
{
    #region Fields

    private readonly ILocalizationService _localizationService = localizationService;

    private readonly IRepository<MailTemplate> _mailTemplateRepository = MailTemplateRepository;

    #endregion
    #region Ctor

    #endregion
    /// <summary>
    /// Gets GetById
    /// </summary>
    /// <returns>Task&lt;MailTemplate&gt;.</returns>
    public virtual async Task<MailTemplate> GetById(int id)
    {
        return await _mailTemplateRepository.GetById(id);
    }

    /// <summary>
    /// Gets GetByTxcodeAndApp
    /// </summary>
    /// <returns>Task&lt;MailTemplateModel&gt;.</returns>
    public virtual async Task<List<MailTemplate>> GetAll()
    {
        return await _mailTemplateRepository.Table.ToListAsync();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public virtual async Task<IPagedList<MailTemplate>> Search(SimpleSearchModel model)
    {
        return await _mailTemplateRepository.GetAllPaged(
            query =>
            {
                if (!string.IsNullOrEmpty(model.SearchText))
                {
                    query = query.Where(c =>
                        c.TemplateId.Contains(model.SearchText)
                        || c.Description.ToString().Contains(model.SearchText)
                        || c.Body.Contains(model.SearchText)
                        || c.DataSample.Contains(model.SearchText)
                        || c.Subject.Contains(model.SearchText)
                        || c.Status.Contains(model.Status)
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
    /// <param name="MailTemplate"></param>
    /// <returns></returns>
    public virtual async Task<MailTemplate> Insert(MailTemplate MailTemplate)
    {
        await _mailTemplateRepository.Insert(MailTemplate);
        return MailTemplate;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="MailTemplate"></param>
    /// <returns></returns>
    public virtual async Task<MailTemplate> Update(MailTemplate MailTemplate)
    {
        await _mailTemplateRepository.Update(MailTemplate);
        return MailTemplate;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="MailTemplate"></param>
    /// <returns></returns>SmtpClient
    public virtual async Task<MailTemplate> Delete(MailTemplate MailTemplate)
    {
        await _mailTemplateRepository.Delete(MailTemplate);
        return MailTemplate;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="templateId"></param>
    /// <returns></returns>
    public virtual async Task<MailTemplate> GetByTemplateId(string templateId)
    {
        return await _mailTemplateRepository
            .Table.Where(s => s.TemplateId.Equals(templateId))
            .FirstOrDefaultAsync();
    }
}
