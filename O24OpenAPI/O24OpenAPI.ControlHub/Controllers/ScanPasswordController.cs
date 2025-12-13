using Microsoft.AspNetCore.Mvc;
using O24OpenAPI.ControlHub.Services.Interfaces;

namespace O24OpenAPI.ControlHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ScanPasswordController : ControllerBase
{
    private readonly IUserAccountService _accountService;

    public ScanPasswordController(IUserAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("scanpassword")]
    public async Task<IActionResult> ScanPassword([FromQuery] string password)
    {
        var result = await _accountService.ScanPassword(password);
        return Ok(new
        {
            message = "ScanPassword completed",
            result.TotalMigrated,
            result.TotalFailed,
            result.Errors
        });
    }
}
