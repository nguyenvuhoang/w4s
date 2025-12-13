using Microsoft.AspNetCore.Mvc;
using O24OpenAPI.ControlHub.Services.Interfaces;

namespace O24OpenAPI.ControlHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ScanRoleController : ControllerBase
{
    private readonly IUserAccountService _accountService;

    public ScanRoleController(IUserAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("scanrole")]
    public async Task<IActionResult> ScanRole([FromQuery] int roleid)
    {
        var result = await _accountService.ScanRole(roleid);
        return Ok(new
        {
            message = "ScanRole completed",
            result.TotalMigrated,
            result.TotalFailed,
            result.Errors
        });
    }
}
