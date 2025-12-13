using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.ControlHub.Services.Interfaces;
using O24OpenAPI.Core;
using O24OpenAPI.Data;
using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.Web.Framework.Models;

namespace O24OpenAPI.ControlHub.Services;

public class TransactionDefinitionService : ITransactionDefinitionService
{
    private readonly IRepository<TransactionDefinition> _transactionDefinitionRepository;
    private readonly StringComparison ICIC = StringComparison.InvariantCultureIgnoreCase;
    /// <summary>
    /// Get all transaction definitions from the database.
    /// </summary>
    /// <returns></returns>
    public async Task<IList<TransactionDefinition>> GetAll()
    {
        var transactionDefinitions = await _transactionDefinitionRepository.GetAll(query =>
        {

            return query;
        });

        return transactionDefinitions;
    }

    /// <summary>
    /// Get a transaction definition by its ID.
    /// </summary>
    /// <param name="transactionDefinitionId"></param>
    /// <returns></returns>
    public async Task<TransactionDefinition> GetById(int transactionDefinitionId)
    {
        return await _transactionDefinitionRepository.GetById(transactionDefinitionId, cache => default);
    }

    /// <summary>
    /// Insert a new transaction definition into the database.
    /// </summary>
    /// <param name="transactionDefinition"></param>
    /// <returns></returns>
    public async Task<TransactionDefinition> InsertTranDef(TransactionDefinition transactionDefinition)
    {
        await _transactionDefinitionRepository.Insert(transactionDefinition);
        return transactionDefinition;
    }

    /// <summary>
    /// Search for transaction definitions based on a simple search model.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<IPagedList<TransactionDefinition>> Search(SimpleSearchModel model)
    {
        model.PageSize = model.PageSize == 0 ? int.MaxValue : model.PageSize;
        if (model.SearchText == null)
        {
            model.SearchText = string.Empty;
        }

        var TransDefs = _transactionDefinitionRepository.Table.Where(c =>
                   c.TransactionCode.Contains(model.SearchText, ICIC)
                || c.WorkflowId.Contains(model.SearchText, ICIC)
                || c.TransactionName.Contains(model.SearchText, ICIC)
                || c.Description.Contains(model.SearchText, ICIC)
                || c.TransactionType.Contains(model.SearchText, ICIC)
                || c.ServiceId.Contains(model.SearchText, ICIC)
                || c.MessageAccount.Contains(model.SearchText, ICIC)
                || c.MessageAmount.Contains(model.SearchText, ICIC)
                || c.MessageCurrency.Contains(model.SearchText, ICIC)
                || c.Voucher.Contains(model.SearchText, ICIC)
                || c.ApplicationCode.Contains(model.SearchText, ICIC)
                || c.TransactionModel.Contains(model.SearchText, ICIC)
                || c.Channel.Contains(model.SearchText, ICIC));
        var PaegeTransDefs = await TransDefs.ToPagedList(model.PageIndex, model.PageSize);
        return PaegeTransDefs;
    }
}
