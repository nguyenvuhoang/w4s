namespace O24OpenAPI.O24ACT.Services;

/// <summary>
/// ICheckingAccountRulesService
/// </summary>
public partial interface ICheckingAccountRulesService
{
    /// <summary>
    /// IsClassificationValid
    /// </summary>
    /// <param name="accls"></param>
    /// <returns></returns>
    bool IsClassificationValid(string accls);
    /// <summary>
    ///
    /// </summary>
    /// <param name="accls"></param>
    /// <param name="rbal"></param>
    /// <returns></returns>
    bool IsReverseBalanceValid(string accls, string rbal);
    /// <summary>
    /// IsBalanceSideValid
    /// </summary>
    /// <param name="accls"></param>
    /// <param name="rbal"></param>
    /// <param name="bside"></param>
    /// <returns></returns>
    bool IsBalanceSideValid(string accls, string rbal, string bside);
    /// <summary>
    /// IsPostingSideValid
    /// </summary>
    /// <param name="accls"></param>
    /// <param name="rbal"></param>
    /// <param name="bside"></param>
    /// <param name="pside"></param>
    /// <returns></returns>
    bool IsPostingSideValid(string accls, string rbal, string bside, string pside);
    /// <summary>
    /// IsAccountGroupValid
    /// </summary>
    /// <param name="accls"></param>
    /// <param name="rbal"></param>
    /// <param name="bside"></param>
    /// <param name="pside"></param>
    /// <param name="acgrp"></param>
    /// <returns></returns>
    bool IsAccountGroupValid(string accls, string rbal, string bside, string pside, string acgrp);
    /// <summary>
    /// IsAccountCategoriesValid
    /// </summary>
    /// <param name="accls"></param>
    /// <param name="rbal"></param>
    /// <param name="bside"></param>
    /// <param name="pside"></param>
    /// <param name="acgrp"></param>
    /// <param name="accat"></param>
    /// <returns></returns>
    bool IsAccountCategoriesValid(string accls, string rbal, string bside, string pside, string acgrp, string accat);
    /// <summary>
    /// CheckingAccountRule
    /// </summary>
    /// <param name="accls"></param>
    /// <param name="rbal"></param>
    /// <param name="bside"></param>
    /// <param name="pside"></param>
    /// <param name="acgrp"></param>
    /// <param name="accat"></param>
    /// <returns></returns>
    Task CheckingRuleAccount(string accls, string rbal, string bside, string pside, string acgrp, string accat);


}
