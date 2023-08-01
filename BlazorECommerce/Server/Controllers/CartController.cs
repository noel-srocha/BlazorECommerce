﻿using Microsoft.AspNetCore.Mvc;

namespace BlazorECommerce.Server.Controllers;

using Shared.DTOs;
using System.Security.Claims;

[Route("api/[controller]")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpPost("products")]
    public async Task<ActionResult<ServiceResponse<List<CartProductResponseDTO>>>> GetCartProducts(List<CartItem> cartItems)
    {
        var result = await _cartService.GetCartProducts(cartItems);

        return Ok(result);
    }
    
    [HttpPost]
    public async Task<ActionResult<ServiceResponse<List<CartProductResponseDTO>>>> StoreCartItems(List<CartItem> cartItems)
    {
        var result = await _cartService.StoreCartItems(cartItems);

        return Ok(result);
    }

    [HttpGet("count")]
    public async Task<ActionResult<ServiceResponse<int>>> GetCartItemsCount()
    {
        return await _cartService.GetCartItemsCount();
    }
}