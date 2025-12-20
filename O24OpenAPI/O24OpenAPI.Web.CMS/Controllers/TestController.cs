using Microsoft.AspNetCore.Mvc;
using O24OpenAPI.Web.CMS.Services.Interfaces;
using O24OpenAPI.Framework.Controllers;

namespace O24OpenAPI.Web.CMS.Controllers;

public class TestController(IBranchService branchService) : BaseController
{
    private readonly IBranchService _branchService = branchService;

    [HttpPost]
    public virtual async Task<IActionResult> UpdateBranch(string branchId)
    {
        var branch = await _branchService.GetByBranchCode(branchId);
        branch.MobilePhone = "linh nè";
        await _branchService.Update(branch);
        return Ok(branch);
    }

    [HttpPost]
    public virtual async Task<IActionResult> PrintDuplicateWFIds(string path)
    {
        var entities = await O24OpenAPI.Data.Utils.FileUtils.ReadJson<WorkflowStep>(path);

        var duplicateWFIds = entities
            .Where(e => !string.IsNullOrEmpty(e.WFId))
            .GroupBy(e => e.WFId)
            .Where(group => group.Count() > 1)
            .Select(group => group.Key)
            .ToList();

        Console.WriteLine("Các WFId bị trùng");
        foreach (var wfId in duplicateWFIds)
        {
            Console.WriteLine(wfId);
        }
        return Ok(duplicateWFIds);
    }
}
