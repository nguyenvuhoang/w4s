using O24OpenAPI.Core;
using O24OpenAPI.Framework.Localization;
using O24OpenAPI.O24ACT.Domain;
using O24OpenAPI.O24ACT.Services.Interfaces;

namespace O24OpenAPI.O24ACT.Services;

/// <summary>
/// CheckingAccountRulesService
/// </summary>
public partial class CheckingAccountRulesService : ICheckingAccountRulesService
{
    #region  Fields
    private readonly IRepository<CheckingAccountRules> _checkingAccountRulesRepository;

    #endregion

    #region Ctor
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="checkingAccountRulesRepository"></param>
    /// <param name="localizationService"></param>
    public CheckingAccountRulesService(
        IRepository<CheckingAccountRules> checkingAccountRulesRepository,
        ILocalizationService localizationService
    )
    {
        _checkingAccountRulesRepository = checkingAccountRulesRepository;
    }

    #endregion
    /// <summary>
    /// IsClassificationValid
    /// </summary>
    /// <param name="accls"></param>
    /// <returns></returns>
    public virtual bool IsClassificationValid(string accls)
    {
        var rule = _checkingAccountRulesRepository
            .Table.Where(c => c.AccountClassification.ToUpper().Equals(accls.ToUpper()))
            .FirstOrDefault();
        return (rule != null);
    }

    /// <summary>
    /// IsReverseBalanceValid
    /// </summary>
    /// <param name="accls"></param>
    /// <param name="rbal"></param>
    /// <returns></returns>
    public virtual bool IsReverseBalanceValid(string accls, string rbal)
    {
        var rule = _checkingAccountRulesRepository
            .Table.Where(c =>
                c.AccountClassification.ToUpper().Equals(accls.ToUpper())
                && c.ReverseBalance.ToUpper().Equals(rbal.ToUpper())
            )
            .FirstOrDefault();
        return (rule != null);
    }

    /// <summary>
    /// IsBalanceSideValid
    /// </summary>
    /// <param name="accls"></param>
    /// <param name="rbal"></param>
    /// <param name="bside"></param>
    /// <returns></returns>
    public virtual bool IsBalanceSideValid(string accls, string rbal, string bside)
    {
        var rule = _checkingAccountRulesRepository
            .Table.Where(c =>
                c.AccountClassification.ToUpper().Equals(accls.ToUpper())
                && c.ReverseBalance.ToUpper().Equals(rbal.ToUpper())
                && c.BalanceSide.ToUpper().Equals(bside.ToUpper())
            )
            .FirstOrDefault();
        return (rule != null);
    }

    /// <summary>
    /// IsPostingSideValid
    /// </summary>
    /// <param name="accls"></param>
    /// <param name="rbal"></param>
    /// <param name="bside"></param>
    /// <param name="pside"></param>
    /// <returns></returns>
    public virtual bool IsPostingSideValid(string accls, string rbal, string bside, string pside)
    {
        var rule = _checkingAccountRulesRepository
            .Table.Where(c =>
                c.AccountClassification.ToUpper().Equals(accls.ToUpper())
                && c.ReverseBalance.ToUpper().Equals(rbal.ToUpper())
                && c.BalanceSide.ToUpper().Equals(bside.ToUpper())
                && c.PostingSide.ToUpper().Equals(pside.ToUpper())
            )
            .FirstOrDefault();
        return (rule != null);
    }

    /// <summary>
    /// IsAccountGroupValid
    /// </summary>
    /// <param name="accls"></param>
    /// <param name="rbal"></param>
    /// <param name="bside"></param>
    /// <param name="pside"></param>
    /// <param name="acgrp"></param>
    /// <returns></returns>
    public virtual bool IsAccountGroupValid(
        string accls,
        string rbal,
        string bside,
        string pside,
        string acgrp
    )
    {
        var rule = _checkingAccountRulesRepository
            .Table.Where(c =>
                c.AccountClassification.ToUpper().Equals(accls.ToUpper())
                && c.ReverseBalance.ToUpper().Equals(rbal.ToUpper())
                && c.BalanceSide.ToUpper().Equals(bside.ToUpper())
                && c.PostingSide.ToUpper().Equals(pside.ToUpper())
                && c.AccountGroup.ToUpper().Equals(acgrp.ToUpper())
            )
            .FirstOrDefault();
        return (rule != null);
    }

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
    public virtual bool IsAccountCategoriesValid(
        string accls,
        string rbal,
        string bside,
        string pside,
        string acgrp,
        string accat
    )
    {
        var rule = _checkingAccountRulesRepository
            .Table.Where(c =>
                c.AccountClassification.ToUpper().Equals(accls.ToUpper())
                && c.ReverseBalance.ToUpper().Equals(rbal.ToUpper())
                && c.BalanceSide.ToUpper().Equals(bside.ToUpper())
                && c.PostingSide.ToUpper().Equals(pside.ToUpper())
                && c.AccountGroup.ToUpper().Equals(acgrp.ToUpper())
                && c.AccountCategories.ToUpper().Equals(accat.ToUpper())
            )
            .FirstOrDefault();
        return (rule != null);
    }

    /// <summary>
    /// CheckingRuleAccount
    /// </summary>
    /// <param name="accls"></param>
    /// <param name="rbal"></param>
    /// <param name="bside"></param>
    /// <param name="pside"></param>
    /// <param name="acgrp"></param>
    /// <param name="accat"></param>
    /// <returns></returns>
    /// <exception cref="NeptuneException"></exception>
    public virtual async Task CheckingRuleAccount(
        string accls,
        string rbal,
        string bside,
        string pside,
        string acgrp,
        string accat
    )
    {
        var ruleClassification = await _checkingAccountRulesRepository.GetAll(query =>
        {
            query = query.Where(c => c.AccountClassification.ToUpper().Equals(accls.ToUpper()));
            return query;
        });

        if (ruleClassification.Count == 0)
        {
            throw new O24OpenAPIException("Invalid field name Account Classification");
        }

        var ruleReverseBalance = ruleClassification.Where(c =>
            c.ReverseBalance.ToUpper().Equals(rbal.ToUpper())
        );
        if (ruleReverseBalance.ToList().Count == 0)
        {
            throw new O24OpenAPIException("Invalid field name Reverse Balance");
        }

        var ruleBalanceSide = ruleReverseBalance.Where(c =>
            c.BalanceSide.ToUpper().Equals(bside.ToUpper())
        );
        if (ruleBalanceSide.ToList().Count == 0)
        {
            throw new O24OpenAPIException("Invalid field name Balance Side");
        }

        var rulePostingSide = ruleBalanceSide.Where(c =>
            c.PostingSide.ToUpper().Equals(pside.ToUpper())
        );
        if (rulePostingSide.ToList().Count == 0)
        {
            throw new O24OpenAPIException("Invalid field name Posting Side");
        }

        var ruleAccountGroup = rulePostingSide.Where(c =>
            c.AccountGroup.ToUpper().Equals(acgrp.ToUpper())
        );
        if (ruleAccountGroup.ToList().Count == 0)
        {
            throw new O24OpenAPIException("Invalid field name Account Group");
        }

        var ruleAccountCategories = ruleAccountGroup.Where(c =>
            c.AccountCategories.ToUpper().Equals(accat.ToUpper())
        );
        if (ruleAccountCategories.ToList().Count == 0)
        {
            throw new O24OpenAPIException("Invalid field name Account Categories");
        }
    }
}
