using O24OpenAPI.Core;
using O24OpenAPI.O24ACT.Common;
using O24OpenAPI.O24ACT.Models;
using O24OpenAPI.O24ACT.Services.Interfaces;

namespace O24OpenAPI.O24ACT.Services;

public class ValidatorService(IAccountChartService accountChartService) : IValidatorService
{
    private readonly IAccountChartService _accountChartService = accountChartService;

    public async Task FundTransferValidator(FundTransferModel model)
    {
        if (model.TransferData.Count != 0 && !model.IsReverse)
        {
            string error = string.Empty;

            var lstGroup = model.TransferData.Select(x => x.GroupId).Distinct().ToList();
            foreach (var groupId in lstGroup)
            {
                var lstTransferByGroup = model.TransferData.Where(x => x.GroupId == groupId);

                foreach (var item in lstTransferByGroup.Select(x => new { x.AccountNumber, x.PostingSide, x.CurrencyCode, x.ModuleCode }))
                {
                    if (item.ModuleCode == Constants.Module.Accounting)
                    {
                        var act = await _accountChartService.GetByAccountNumber(item.AccountNumber);
                        if (act == null)
                        {
                            throw new O24OpenAPIException("Accounting Account Invalid", item.AccountNumber);

                        }
                        else
                        {
                            if (act.CurrencyCode != item.CurrencyCode)
                            {
                                throw new O24OpenAPIException("Accounting InvalidCurrency");
                            }

                            if (act.PostingSide != item.PostingSide)
                            {
                                if (act.PostingSide == "D")
                                {
                                    throw new O24OpenAPIException("Accounting Not Allowed Credit Side");
                                }
                                else if (act.PostingSide == "C")
                                {
                                    throw new O24OpenAPIException("Accounting Not Allowed Debit Side", error);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
