using O24OpenAPI.O24ACT.Domain;
using O24OpenAPI.Web.Framework.Models;

namespace O24OpenAPI.O24ACT.Models;

public partial class AccountChartInforItemModel : BaseO24OpenAPIModel
{
    /// <summary>
    ///
    /// </summary>
    public AccountChartInforItemModel()
    {
        AccountBalance = new AccountBalance();
        AccountChart = new AccountChart();
    }
    /// <summary>
    ///
    /// </summary>
    /// <param name="chart"></param>
    /// <param name="balance"></param>
    public AccountChartInforItemModel(AccountChart chart, AccountBalance balance)
    {
        this.AccountChart = chart;
        this.AccountBalance = balance;
    }
    /// <summary>
    /// EntryJournal
    /// </summary>
    public AccountBalance AccountBalance { get; set; }
    /// <summary>
    /// AccountChart
    /// </summary>
    public AccountChart AccountChart { get; set; }
}
