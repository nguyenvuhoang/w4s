using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Core;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.NCH.API.Application.Models.Response;
using O24OpenAPI.NCH.Domain.AggregatesModel.MailAggregate;

namespace O24OpenAPI.NCH.API.Application.Features.MailConfigs;

public class SimpleSearchMailConfigCommand
    : SimpleSearchModel,
        ICommand<PagedListModel<MailConfig, MailConfigResponse>>
{ }

public class SimpleSearchMailConfigHandler(IMailConfigRepository mailConfigRepository)
    : ICommandHandler<SimpleSearchMailConfigCommand, PagedListModel<MailConfig, MailConfigResponse>>
{
    private readonly IMailConfigRepository _mailConfigRepository = mailConfigRepository;

    [WorkflowStep(WorkflowStepCode.NCH.WF_STEP_NCH_SEARCH_MAIL_CONFIG)]
    public async Task<PagedListModel<MailConfig, MailConfigResponse>> HandleAsync(
        SimpleSearchMailConfigCommand request,
        CancellationToken cancellationToken = default
    )
    {
        IPagedList<MailConfig> mailConfigs = await _mailConfigRepository.GetAllPaged(
            query =>
            {
                if (!string.IsNullOrEmpty(request.SearchText))
                {
                    query = query.Where(c =>
                        c.Host.Contains(request.SearchText)
                        || c.Port.ToString().Contains(request.SearchText)
                        || c.Sender.Contains(request.SearchText)
                    );
                }

                query = query.OrderBy(c => c.Id);
                return query;
            },
            0,
            0
        );
        return new PagedListModel<MailConfig, MailConfigResponse>(mailConfigs);
    }
}
