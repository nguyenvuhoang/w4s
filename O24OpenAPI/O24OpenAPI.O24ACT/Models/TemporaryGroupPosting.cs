using O24OpenAPI.Web.Framework.Models;

namespace O24OpenAPI.O24ACT.Models;

public partial class TemporaryGroupPosting : BaseO24OpenAPIModel
{
    /// <summary>
    ///
    /// </summary>
    public List<TemporaryPosting> Postings { get; set; } = new();

    /// <summary>
    ///
    /// </summary>
    public bool IgnoreIBT { get; set; } = false;

    /// <summary>
    ///
    /// </summary>
    public int GroupIndex { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string BaseCurrency { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string HostBranch { get; set; }

    /// <summary>
    /// Check if postings in group has IBT
    /// </summary>
    /// <value></value>
    public bool HasIBT
    {
        get
        {
            return Postings.Select(p => p.BranchGLBankAccountNumber).Distinct().ToList().Count > 1;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public string Branch1
    {
        get
        {
            return Postings.OrderBy(p => p.BranchGLBankAccountNumber == HostBranch ? 0 : 1).Select(p => p.BranchGLBankAccountNumber).Distinct().FirstOrDefault();
        }
    }

    /// <summary>
    ///
    /// </summary>
    public bool HasFX
    {
        get
        {
            return Postings.Select(p => p.CurrencyCodeGLBankAccountNumber).Distinct().ToList().Count > 1;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public void ExpandItems()
    {
        GroupIndex *= 100;
        Postings.ForEach(p =>
        {
            p.AccountingEntryGroup = GroupIndex;
            p.AccountingEntryIndex *= 100;
        });
    }

    /// <summary>
    ///
    /// </summary>
    public void SortItems()
    {
        var ps = Postings.Where(p => p.Amount != 0).OrderBy(p => p.CurrencyCodeGLBankAccountNumber == BaseCurrency ? 1 : 0)
            .ThenBy(p => p.AccountingEntryIndex)
            .ThenBy(p => p.DebitOrCredit == "D" ? 0 : 1).ToList();
        int index = 1;
        foreach (var p in ps)
        {
            p.AccountingEntryGroup = GroupIndex;
            p.AccountingEntryIndex = index;
            index++;
        }
        Postings = ps;
    }
}
