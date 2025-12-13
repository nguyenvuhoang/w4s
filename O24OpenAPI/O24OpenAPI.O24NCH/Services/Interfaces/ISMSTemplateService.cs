using O24OpenAPI.O24NCH.Domain;
using O24OpenAPI.O24NCH.Models.SMSTemplate;

namespace O24OpenAPI.O24NCH.Services.Interfaces;

/// <summary>
/// The id bankservice interface
/// </summary>

public interface ISMSTemplateService
{
    Task<SMSTemplate> GetByTemplateCode(string templateCode);

    Task<SMSTemplate> GetById(int id);

    Task<bool> Insert(SMSTemplateModel model);

    Task<UpdateSMSTemplateResponseModel> Update(UpdateSMSTemplateRequestModel model);
}
