using LinKit.Core.Cqrs;
using LinqToDB;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Core;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.NCH.Domain.AggregatesModel.SmsAggregate;

namespace O24OpenAPI.NCH.API.Application.Features.SMSTemplates;

public class InsertSMSTemplateCommand : BaseTransactionModel, ICommand<bool>
{
    public string TemplateCode { get; set; } = string.Empty;
    public string MessageContent { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}

[CqrsHandler]
public class InsertSMSTemplateHandler(ISMSTemplateRepository sMSTemplateRepository)
    : ICommandHandler<InsertSMSTemplateCommand, bool>
{
    [WorkflowStep(WorkflowStep.NCH.WF_STEP_NCH_INSERT_SMS_TEMPLATE)]
    public async Task<bool> HandleAsync(
        InsertSMSTemplateCommand request,
        CancellationToken cancellationToken = default
    )
    {
        SMSTemplate template = await sMSTemplateRepository
            .Table.Where(s => s.TemplateCode.Equals(request.TemplateCode))
            .FirstOrDefaultAsync();
        if (template != null)
        {
            throw new O24OpenAPIException("This SMS Template is already exists!");
        }
        else
        {
            var newTemplate = new SMSTemplate
            {
                TemplateCode = request.TemplateCode,
                MessageContent = request.MessageContent,
                Description = request.Description,
                IsActive = request.IsActive,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow,
            };
            await sMSTemplateRepository.InsertAsync(newTemplate);
            return true;
        }
    }
}
