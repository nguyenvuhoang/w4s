using O24OpenAPI.Core.Domain;
using O24OpenAPI.O24ACT.Domain;
using O24OpenAPI.O24ACT.Services.Interfaces;
using O24OpenAPI.Web.Framework.Domain;
using O24OpenAPI.Web.Framework.Extensions;

namespace O24OpenAPI.O24ACT.Services;

public class AccountingService(
    IRepository<GLEntries> glEntriesRepository,
    IRepository<GLEntriesDone> glEntriesDoneRepository,
    IRepository<AccountStatement> accountStatementRepository,
    IRepository<AccountStatementDone> accountStatementDoneRepository)
    : IAccountingService
{
    private readonly IRepository<GLEntries> _glEntriesRepository = glEntriesRepository;
    private readonly IRepository<GLEntriesDone> _glEntriesDoneRepository = glEntriesDoneRepository;
    private readonly IRepository<AccountStatement> _accountStatementRepository = accountStatementRepository;
    private readonly IRepository<AccountStatementDone> _accountStatementDoneRepository = accountStatementDoneRepository;


    private const int BatchSize = 500;

    public Task SyncGLEntries() => SyncGLEntries(DateTime.UtcNow.Date, CancellationToken.None);

    /// <summary>
    /// Sync GL Entries to GLEntriesDone table
    /// </summary>
    /// <param name="workingDate"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async Task SyncGLEntries(DateTime workingDate, CancellationToken ct = default)
    {
        try
        {
            var eligible = _glEntriesRepository.Table
                .Where(x => x.Posted == true && x.ValueDate < workingDate)
                .OrderBy(x => x.Id);

            int lastId = 0;

            while (true)
            {
                ct.ThrowIfCancellationRequested();

                var batch = eligible
                    .Where(x => x.Id > lastId)
                    .Take(BatchSize)
                    .Select(x => new
                    {
                        x.Id,
                        x.TransactionNumber,
                        x.TransTableName,
                        x.TransId,
                        x.SysAccountName,
                        x.GLAccount,
                        x.DorC,
                        x.TransactionStatus,
                        x.Amount,
                        x.BranchCode,
                        x.CurrencyCode,
                        x.CrossBranchCode,
                        x.CrossCurrencyCode,
                        x.BaseCurrencyAmount,
                        x.ValueDate,
                        x.Posted,
                        x.AccountingGroup
                    })
                    .ToList();

                if (batch.Count == 0)
                {
                    break;
                }

                try
                {
                    var doneRows = new List<GLEntriesDone>(batch.Count);
                    foreach (var it in batch)
                    {
                        doneRows.Add(new GLEntriesDone
                        {
                            TransactionNumber = it.TransactionNumber,
                            TransTableName = it.TransTableName,
                            TransId = it.TransId,
                            SysAccountName = it.SysAccountName,
                            GLAccount = it.GLAccount,
                            DorC = it.DorC,
                            TransactionStatus = it.TransactionStatus,
                            Amount = it.Amount,
                            BranchCode = it.BranchCode,
                            CurrencyCode = it.CurrencyCode,
                            CrossBranchCode = it.CrossBranchCode,
                            CrossCurrencyCode = it.CrossCurrencyCode,
                            BaseCurrencyAmount = it.BaseCurrencyAmount,
                            ValueDate = it.ValueDate,
                            Posted = it.Posted,
                            AccountingGroup = it.AccountingGroup
                        });
                    }

                    await InsertRangeSafeAsync(_glEntriesDoneRepository, doneRows, ct);
                    var glIds = batch.Select(b => b.Id).ToList();
                    await DeleteRangeSafeAsync(_glEntriesRepository, glIds, ct);

                    lastId = batch[^1].Id;
                }
                catch (Exception ex)
                {
                    await ex.LogErrorAsync();
                    break;
                }
            }

            try
            {
                var stEligible = _accountStatementRepository.Table
                    .Where(x => x.ValueDate < workingDate)
                    .OrderBy(x => x.Id);

                int lastStId = 0;

                while (true)
                {
                    ct.ThrowIfCancellationRequested();

                    var stBatch = stEligible
                        .Where(x => x.Id > lastStId)
                        .Take(BatchSize)
                        .Select(x => new
                        {
                            x.Id,
                            x.AccountNumber,
                            x.CurrencyCode,
                            x.ConvertAmount,
                            x.Amount,
                            x.ReferenceId,
                            x.StatementStatus,
                            x.StatementDate,
                            x.ValueDate,
                            x.StatementCode,
                            x.RefNumber,
                            x.TransCode,
                            x.Description,
                            x.CreatedOnUtc,
                            x.UpdatedOnUtc,
                            x.TransactionNumber
                        })
                        .ToList();

                    if (stBatch.Count == 0)
                    {
                        break;
                    }

                    try
                    {
                        var stDoneRows = new List<AccountStatementDone>(stBatch.Count);
                        foreach (var it in stBatch)
                        {
                            stDoneRows.Add(new AccountStatementDone
                            {
                                AccountNumber = it.AccountNumber,
                                CurrencyCode = it.CurrencyCode,
                                ConvertAmount = it.ConvertAmount,
                                Amount = it.Amount,
                                ReferenceId = it.ReferenceId,
                                StatementStatus = it.StatementStatus,
                                StatementDate = it.StatementDate,
                                ValueDate = it.ValueDate,
                                StatementCode = it.StatementCode,
                                RefNumber = it.RefNumber,
                                TransCode = it.TransCode,
                                Description = it.Description,
                                CreatedOnUtc = it.CreatedOnUtc,
                                UpdatedOnUtc = it.UpdatedOnUtc,
                                TransactionNumber = it.TransactionNumber
                            });
                        }

                        await InsertRangeSafeAsync(_accountStatementDoneRepository, stDoneRows, ct);
                        var stIds = stBatch.Select(b => b.Id).ToList();
                        await DeleteRangeSafeAsync(_accountStatementRepository, stIds, ct);

                        lastStId = stBatch[^1].Id;
                    }
                    catch (Exception ex)
                    {
                        await ex.LogErrorAsync($"[SyncGLEntries] Error processing AccountStatement batch starting from Id {lastStId}: {ex.Message}");
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                await ex.LogErrorAsync($"[SyncGLEntries] Error in AccountStatement processing loop: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync($"[SyncGLEntries] Fatal error: {ex.Message}");
            throw;
        }
    }


    private static async Task InsertRangeSafeAsync<T>(
        IRepository<T> repo,
        IEnumerable<T> items,
        CancellationToken ct) where T : BaseEntity
    {
        var list = items?.ToList() ?? [];
        if (list.Count == 0)
        {
            return;
        }

        var bulkInsertMethod = repo.GetType().GetMethod(
            "BulkInsert",
            [typeof(IList<T>), typeof(string), typeof(bool)]);

        if (bulkInsertMethod != null)
        {
            var task = (Task)bulkInsertMethod.Invoke(repo, [list, string.Empty, false])!;
            await task;
            return;
        }

        var insertRange = repo.GetType().GetMethod(
            "InsertAsync",
            [typeof(IEnumerable<T>), typeof(CancellationToken)]);

        if (insertRange != null)
        {
            var task = (Task)insertRange.Invoke(repo, [list, ct])!;
            await task;
            return;
        }

        foreach (var item in list)
        {
            ct.ThrowIfCancellationRequested();
            await repo.InsertAsync(item);
        }
    }

    private static async Task DeleteRangeSafeAsync<T>(
        IRepository<T> repo,
        List<int> ids,
        CancellationToken ct) where T : BaseEntity
    {
        if (ids == null || ids.Count == 0)
        {
            return;
        }

        var idSet = new HashSet<int>(ids);
        var toDelete = repo.Table.Where(x => idSet.Contains(x.Id)).ToList();
        if (toDelete.Count == 0)
        {
            return;
        }

        var bulkDelete3 = repo.GetType().GetMethod(
            "BulkDelete",
            [typeof(IList<T>), typeof(string), typeof(bool)]);

        if (bulkDelete3 != null)
        {
            var task = (Task)bulkDelete3.Invoke(repo, [toDelete, string.Empty, false])!;
            await task;
            return;
        }

        var bulkDelete1 = repo.GetType().GetMethod(
            "BulkDelete",
            [typeof(IList<T>)]);

        if (bulkDelete1 != null)
        {
            var task = (Task)bulkDelete1.Invoke(repo, [toDelete])!;
            await task;
            return;
        }

        var delRange = repo.GetType().GetMethod(
            "DeleteAsync",
            [typeof(IEnumerable<T>), typeof(CancellationToken)]);

        if (delRange != null)
        {
            var task = (Task)delRange.Invoke(repo, [toDelete, ct])!;
            await task;
            return;
        }

        var delOneWithCt = repo.GetType().GetMethod("DeleteAsync", [typeof(T), typeof(CancellationToken)]);
        if (delOneWithCt != null)
        {
            foreach (var e in toDelete)
            {
                ct.ThrowIfCancellationRequested();
                await (Task)delOneWithCt.Invoke(repo, [e, ct])!;
            }
            return;
        }

        foreach (var e in toDelete)
        {
            ct.ThrowIfCancellationRequested();
            await repo.Delete(e);
        }
    }
}
