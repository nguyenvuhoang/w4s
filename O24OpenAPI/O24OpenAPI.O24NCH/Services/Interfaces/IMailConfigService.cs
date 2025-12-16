using O24OpenAPI.Core;
using O24OpenAPI.O24NCH.Domain;
using O24OpenAPI.O24NCH.Models.Request;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.O24NCH.Services.Interfaces;

public partial interface IMailConfigService

{  /// <summary>
   /// Gets the by identifier.
   /// </summary>
   /// <param name="id">The identifier.</param>
    Task<MailConfig> GetById(int id);
    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    Task<IPagedList<MailConfig>> Search(SimpleSearchModel model);
    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<IPagedList<MailConfig>> Search(MailConfigSearchModel model);
    /// <summary>
    ///
    /// </summary>
    /// <param name="mailConfig"></param>
    /// <returns></returns>
    Task<MailConfig> Create(MailConfig mailConfig);
    /// <summary>
    ///Update
    /// </summary>
    Task<MailConfigUpdateModel> Update(MailConfigUpdateModel mailConfig, string referenceId = "");
    /// <summary>
    ///
    /// </summary>
    /// <param name="mailConfig"></param>
    /// <returns></returns>
    Task<MailConfig> Delete(MailConfig mailConfig);
    /// <summary>
    ///
    /// </summary>
    /// <param name="configId"></param>
    /// <returns></returns>
    Task<MailConfig> GetByConfigId(string configId);
}
