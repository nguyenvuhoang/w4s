using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Core;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.NCH.API.Application.Models.Response;
using O24OpenAPI.NCH.Domain.AggregatesModel.MailAggregate;

namespace O24OpenAPI.NCH.API.Application.Features.MailTemplates;

public class SimpleSearchMailTemplateCommand
    : SimpleSearchModel,
        ICommand<PagedListModel<MailTemplate, MailTemplateResponse>>
{ }

[CqrsHandler]
public class SimpleSearchMailTemplateHandler(IMailTemplateRepository mailTemplateRepository)
    : ICommandHandler<
        SimpleSearchMailTemplateCommand,
        PagedListModel<MailTemplate, MailTemplateResponse>
    >
{
    [WorkflowStep(WorkflowStepCode.NCH.WF_STEP_NCH_SEARCH_MAIL_TEMPLATE)]
    public async Task<PagedListModel<MailTemplate, MailTemplateResponse>> HandleAsync(
        SimpleSearchMailTemplateCommand request,
        CancellationToken cancellationToken = default
    )
    {
        IPagedList<MailTemplate> pagedMailTemplates = await mailTemplateRepository.GetAllPaged(
            query =>
            {
                if (!string.IsNullOrEmpty(request.SearchText))
                {
                    query = query.Where(c =>
                        c.TemplateId.Contains(request.SearchText)
                        || c.Description.ToString().Contains(request.SearchText)
                        || c.Body.Contains(request.SearchText)
                        || c.DataSample.Contains(request.SearchText)
                        || c.Subject.Contains(request.SearchText)
                        || c.Status.Contains(request.Status)
                    );
                }

                query = query.OrderBy(c => c.Id);
                return query;
            },
            0,
            0
        ); ;
        return new PagedListModel<MailTemplate, MailTemplateResponse>(pagedMailTemplates);
    }
}
