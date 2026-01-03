using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Core.Constants;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Infrastructure.Mapper.Extensions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.NCH.API.Application.Models.SMSTemplate;
using O24OpenAPI.NCH.Domain.AggregatesModel.SmsAggregate;
using System.Reflection;

namespace O24OpenAPI.NCH.API.Application.Features.SMSTemplates;

public class UpdateSMSTemplateCommand
    : BaseTransactionModel,
        ICommand<UpdateSMSTemplateResponseModel>
{
    public int Id { get; set; }
    public string TemplateCode { get; set; } = string.Empty;
    public string MessageContent { get; set; } = string.Empty;
    public bool? IsActive { get; set; }

    public DateTime? CreatedOnUtc { get; set; }

    public DateTime? UpdatedOnUtc { get; set; }

    public List<string> ChangedFields { get; set; } = [];

    public static UpdateSMSTemplateRequestModel FromUpdatedEntity(
        SMSTemplate updated,
        SMSTemplate original
    )
    {
        UpdateSMSTemplateRequestModel result = new();
        PropertyInfo[] entityProps = typeof(SMSTemplate).GetProperties();
        Dictionary<string, PropertyInfo> modelProps = typeof(UpdateSMSTemplateRequestModel)
            .GetProperties()
            .ToDictionary(p => p.Name);

        foreach (PropertyInfo prop in entityProps)
        {
            if (!modelProps.ContainsKey(prop.Name))
            {
                continue;
            }

            object newValue = prop.GetValue(updated);
            object oldValue = prop.GetValue(original);

            if (
                (oldValue == null && newValue != null)
                || (oldValue != null && !oldValue.Equals(newValue))
            )
            {
                result.ChangedFields.Add(prop.Name);
            }

            modelProps[prop.Name].SetValue(result, newValue);
        }

        return result;
    }
}

public class UpdateSMSTemplateResponseModel : BaseO24OpenAPIModel
{
    public UpdateSMSTemplateResponseModel() { }

    public string TemplateCode { get; set; } = string.Empty;
    public string MessageContent { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool? IsActive { get; set; }

    public DateTime? CreatedOnUtc { get; set; }

    public DateTime? UpdatedOnUtc { get; set; }
    public List<string> ChangedFields { get; set; } = [];

    public static UpdateSMSTemplateResponseModel FromUpdatedEntity(
        SMSTemplate updated,
        SMSTemplate original
    )
    {
        UpdateSMSTemplateResponseModel result = new();
        PropertyInfo[] entityProps = typeof(SMSTemplate).GetProperties();
        Dictionary<string, PropertyInfo> modelProps = typeof(UpdateSMSTemplateResponseModel)
            .GetProperties()
            .ToDictionary(p => p.Name);

        foreach (PropertyInfo prop in entityProps)
        {
            if (!modelProps.ContainsKey(prop.Name))
            {
                continue;
            }

            object newValue = prop.GetValue(updated);
            object oldValue = prop.GetValue(original);

            bool isChanged =
                (oldValue == null && newValue != null)
                || (oldValue != null && !oldValue.Equals(newValue));

            if (isChanged)
            {
                result.ChangedFields.Add(prop.Name);
            }

            PropertyInfo targetProp = modelProps[prop.Name];
            if (newValue != null && targetProp.PropertyType != newValue.GetType())
            {
                try
                {
                    Type targetType =
                        Nullable.GetUnderlyingType(targetProp.PropertyType)
                        ?? targetProp.PropertyType;
                    newValue = Convert.ChangeType(newValue, targetType);
                }
                catch
                {
                    continue;
                }
            }

            targetProp.SetValue(result, newValue);
        }

        return result;
    }
}

[CqrsHandler]
public class UpdateSMSTemplateHandler(ISMSTemplateRepository sMSTemplateRepository)
    : ICommandHandler<UpdateSMSTemplateCommand, UpdateSMSTemplateResponseModel>
{
    [WorkflowStep(WorkflowStepCode.NCH.WF_STEP_NCH_UPDATE_SMS_TEMPLATE)]
    public async Task<UpdateSMSTemplateResponseModel> HandleAsync(
        UpdateSMSTemplateCommand request,
        CancellationToken cancellationToken = default
    )
    {
        SMSTemplate entity =
            await sMSTemplateRepository.GetById(request.Id)
            ?? throw await O24Exception.CreateAsync(
                ResourceCode.Common.NotExists,
                request.Language
            );

        SMSTemplate originalEntity = entity.Clone();

        request.ToEntityNullable(entity);

        await sMSTemplateRepository.Update(entity);

        return UpdateSMSTemplateResponseModel.FromUpdatedEntity(entity, originalEntity);
    }
}
