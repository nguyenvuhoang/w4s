using O24OpenAPI.Core;
using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.O24NCH.Domain;
using O24OpenAPI.O24NCH.Models.SMSTemplate;
using O24OpenAPI.O24NCH.Services.Interfaces;
using O24OpenAPI.Web.Framework.Exceptions;
using O24OpenAPI.Web.Framework.Infrastructure.Mapper.Extensions;


namespace O24OpenAPI.O24NCH.Services.Services;

public class SMSTemplateService : ISMSTemplateService
{
    #region Fields

    private readonly IRepository<SMSTemplate> _SMSTemplateRepository;

    #endregion

    #region Ctor

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="localizationService"></param>
    /// <param name="templateTransferRepository"></param>
    public SMSTemplateService(
        IRepository<SMSTemplate> SMSTemplateRepository
    )
    {
        _SMSTemplateRepository = SMSTemplateRepository;
    }

    #endregion

    public virtual async Task<SMSTemplate> GetByTemplateCode(string templateCode)
    {
        return await _SMSTemplateRepository
            .Table.Where(s => s.TemplateCode.Equals(templateCode.Trim()))
            .FirstOrDefaultAsync();
    }

    public virtual async Task<SMSTemplate> GetById(int id)
    {
        return await _SMSTemplateRepository.GetById(id);
    }

    public virtual async Task<bool> Insert(SMSTemplateModel model)
    {
        var template = await _SMSTemplateRepository
            .Table.Where(s => s.TemplateCode.Equals(model.TemplateCode))
            .FirstOrDefaultAsync();
        if (template != null)
        {
            throw new O24OpenAPIException(
                    "This SMS Template is already exists!"
                );
        }
        else
        {
            var newTemplate = new SMSTemplate
            {
                TemplateCode = model.TemplateCode,
                MessageContent = model.MessageContent,
                Description = model.Description,
                IsActive = model.IsActive,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            await _SMSTemplateRepository.Insert(newTemplate);
            return true;
        }
    }

    public async Task<UpdateSMSTemplateResponseModel> Update(UpdateSMSTemplateRequestModel model)
    {
        var entity = await _SMSTemplateRepository.GetById(model.Id)
           ?? throw await O24Exception.CreateAsync(ResourceCode.Common.NotExists, model.Language);

        var originalEntity = entity.Clone();

        model.ToEntityNullable(entity);

        await _SMSTemplateRepository.Update(entity);

        return UpdateSMSTemplateResponseModel.FromUpdatedEntity(entity, originalEntity);
    }
}
