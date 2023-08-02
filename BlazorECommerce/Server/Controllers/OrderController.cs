﻿using Microsoft.AspNetCore.Mvc;

namespace BlazorECommerce.Server.Controllers;

using Services.OrderService;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse<bool>>> PlaceOrder()
    {
        var result = await _orderService.PlaceOrder();
        
        return Ok(result);
    }
}