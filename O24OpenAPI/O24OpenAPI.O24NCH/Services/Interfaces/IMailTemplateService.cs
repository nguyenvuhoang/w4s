using O24OpenAPI.Core;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.O24NCH.Domain;

namespace O24OpenAPI.O24NCH.Services.Interfaces;

public partial interface IMailTemplateService
{
    /// <summary>
    /// Gets the by identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>Task&lt;GetById&gt;.</returns>
    Task<MailTemplate> GetById(int id);

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    Task<List<MailTemplate>> GetAll();

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<IPagedList<MailTemplate>> Search(SimpleSearchModel model);

    /// <summary>
    ///
    /// </summary>
    /// <param name="MailTemplate"></param>
    /// <returns></returns>
    Task<MailTemplate> Insert(MailTemplate MailTemplate);

    /// <summary>
    ///Update
    /// </summary>
    /// <returns>Task&lt;MailTemplate&gt;.</returns>
    Task<MailTemplate> Update(MailTemplate MailTemplate);

    /// <summary>
    ///
    /// </summary>
    /// <param name="MailTemplate"></param>
    /// <returns></returns>
    Task<MailTemplate> Delete(MailTemplate MailTemplate);

    /// <summary>
    ///
    /// </summary>
    /// <param name="templateId"></param>
    /// <returns></returns>
    Task<MailTemplate> GetByTemplateId(string templateId);
}
