using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.Core;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.ControlHub.Services.Interfaces;

public interface ITransactionDefinitionService
{
    /// <summary>
    /// Get a transaction definition by its ID.
    /// </summary>
    /// <param name="transactionDefinitionId"></param>
    /// <returns></returns>
    Task<TransactionDefinition> GetById(int transactionDefinitionId);

    /// <summary>
    /// Get all transaction definitions.
    /// </summary>
    /// <returns></returns>
    Task<IList<TransactionDefinition>> GetAll();

    /// <summary>
    /// Search for transaction definitions based on a simple search model.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<IPagedList<TransactionDefinition>> Search(SimpleSearchModel model);

    /// <summary>
    /// Insert a new transaction definition into the database.
    /// </summary>
    /// <param name="transactionDefinition"></param>
    /// <returns></returns>
    Task<TransactionDefinition> InsertTranDef(TransactionDefinition transactionDefinition);
}
