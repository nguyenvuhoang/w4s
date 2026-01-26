using Microsoft.AspNetCore.Mvc;
using O24OpenAPI.CMS.API.Application.Models;
using O24OpenAPI.Framework.Controllers;
using O24OpenAPI.Kit.Keyvault.Extensions;
using O24OpenAPI.Kit.Signature.Extensions;

namespace O24OpenAPI.CMS.API.Controllers;

public class SignatureController : BaseController
{
    [HttpPost]
    public virtual async Task<IActionResult> GenerateSignature(
        [FromBody] BoRequestModel requestJson
    )
    {
        if (requestJson == null)
        {
            return BadRequest("Request body is required.");
        }

        string privateKey = KeyvaultExtension.GetSecretKey();
        (string signature, string timestamp, string nounce) result = SignatureExtension.GetSignature(requestJson, privateKey);
        (string signature, string timestamp, string nounce) = await Task.FromResult(result);

        return Ok(
            new
            {
                Signature = signature,
                Timestamp = timestamp,
                Nounce = nounce,
            }
        );
    }
}
