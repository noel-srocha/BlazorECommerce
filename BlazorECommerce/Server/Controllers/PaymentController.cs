using Microsoft.AspNetCore.Mvc;

namespace BlazorECommerce.Server.Controllers;

using Microsoft.AspNetCore.Authorization;

[Route("api/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }
    
    [HttpPost]
    public async Task<ActionResult<bool>> FullfillOrder()
    {
        var response = await _paymentService.FulfillOrder(Request);

        if (!response.Success)
            return BadRequest(response.Message);
        
        return Ok(response);
    }

    [HttpPost("checkout"), Authorize]
    public async Task<ActionResult<string>> CreateCheckoutSession()
    {
        var session = await _paymentService.CreateCheckoutSession();
        
        return Ok(session.Url);
    }
}