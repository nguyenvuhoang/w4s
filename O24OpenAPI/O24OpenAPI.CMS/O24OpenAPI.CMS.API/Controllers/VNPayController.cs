using LinKit.Core.Cqrs;
using Microsoft.AspNetCore.Mvc;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.CMS.API.Models.VNPay;
using O24OpenAPI.Framework.Controllers;
namespace O24OpenAPI.CMS.API.Controllers;

public class VNPayController([FromKeyedServices(MediatorKey.Grpc)] IMediator mediator) : BaseController
{
    [HttpPost("/api/vnpay/create")]
    public virtual async Task<IActionResult> Create(
        [FromBody] VNPayProcessPayModel request,
        CancellationToken cancellationToken
    )
    {
        var result = await mediator.SendAsync(request.ToPMTGrpcClientServiceVNPayProcessPayCommand(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("/api/vnpay/return")]
    public async Task<IActionResult> Return(CancellationToken cancellationToken)
    {
        var rawQuery = Request.QueryString.Value ?? string.Empty;

        var cmd = new VNPayProcessReturnModel
        {
            RawQuery = rawQuery
        };


        var request = cmd.ToPMTGrpcClientServiceVNPayProcessReturnCommand();
        var result = await mediator.SendAsync(request, cancellationToken);
        return Ok(result);
    }

    //[HttpGet("/api/vnpay/ipn")]
    //public async Task<IActionResult> IPN(CancellationToken cancellationToken)
    //{
    //    var query = Request.Query;

    //    var vnpParams = query
    //        .Where(kv => kv.Key.StartsWith("vnp_", StringComparison.OrdinalIgnoreCase))
    //        .ToDictionary(kv => kv.Key, kv => kv.Value.ToString());

    //    var vnpay = new VnPayLibrary();
    //    foreach (var kv in vnpParams)
    //    {
    //        if (!string.IsNullOrWhiteSpace(kv.Value))
    //            vnpay.AddResponseData(kv.Key, kv.Value);
    //    }

    //    var cmd = new VNPayProcessIPNCommand
    //    {
    //        Query = vnpParams
    //    };

    //    var result = await mediator.SendAsync(cmd, cancellationToken);
    //    return Ok(result);
    //}

}
