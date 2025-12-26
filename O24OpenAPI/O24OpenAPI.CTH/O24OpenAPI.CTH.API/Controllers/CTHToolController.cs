using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Controllers;

namespace O24OpenAPI.CTH.API.Controllers;

public class CTHToolController : BaseController
{
    [HttpGet]
    public IActionResult GetWorkflowStepConstants()
    {
        var assembly = typeof(Program).Assembly;

        var constants = assembly
            .GetTypes()
            .SelectMany(t =>
                t.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            )
            .SelectMany(m => m.GetCustomAttributes<WorkflowStepAttribute>())
            .Select(a => a.StepCode)
            .Distinct()
            .OrderBy(x => x)
            .Select(code => $"public const string {code} = \"{code}\";")
            .ToList();

        return Ok(constants);
    }
}
