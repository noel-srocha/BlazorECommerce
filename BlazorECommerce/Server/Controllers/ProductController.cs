﻿using Microsoft.AspNetCore.Mvc;

namespace BlazorECommerce.Server.Controllers;

using Microsoft.AspNetCore.Authorization;
using Shared.DTOs;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
	private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
	    _productService = productService;
    }
    
    [HttpPost("admin"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<ServiceResponse<Product>>> CreateProduct(Product product)
    {
	    var result = await _productService.CreateProduct(product);
	      
	    return Ok(result);
    }
    
    [HttpDelete("admin"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<ServiceResponse<bool>>> DeleteProduct(int productId)
    {
	    var result = await _productService.DeleteProduct(productId);
	      
	    return Ok(result);
    }
    
    [HttpGet("admin"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<ServiceResponse<List<Product>>>> GetAdminProducts()
    {
	    var result = await _productService.GetAdminProducts();
	      
	    return Ok(result);
    }
    
    [HttpGet("featured")]
    public async Task<ActionResult<ServiceResponse<List<Product>>>> GetFeaturedProducts()
    {
	    var result = await _productService.GetFeaturedProducts();
	    
	    return Ok(result);
    }
    
    [HttpGet("getproduct/{productId}")]
    public async Task<ActionResult<ServiceResponse<Product>>> GetProduct(int productId)
    {
	    var result = await _productService.GetProduct(productId);

	    return Ok(result);
    }

    [HttpGet("getproducts")]
    public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProducts()
    {
	    var result = await _productService.GetProducts();
	      
	    return Ok(result);
    }
    
    [HttpGet("getproductsbycategory/{categoryUrl}")]
    public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProductsByCategory(string categoryUrl)
    {
	    var result = await _productService.GetProductsByCategory(categoryUrl);

	    return Ok(result);
    }
    
    [HttpGet("searchsuggestions/{searchText}")]
    public async Task<ActionResult<ServiceResponse<List<string>>>> GetProductSearchSuggestions(string searchText)
    {
	    var result = await _productService.GetProductSearchSuggestions(searchText);
	    
	    return Ok(result);
    }

    [HttpGet("search/{searchText}/{page}")]
    public async Task<ActionResult<ServiceResponse<ProductSearchResultDTO>>> SearchProducts(string searchText, int page = 1)
    {
	    var result = await _productService.SearchProducts(searchText, page);

	    return Ok(result);
    }
    
    [HttpPut("admin"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<ServiceResponse<Product>>> UpdateProduct(Product product)
    {
	    var result = await _productService.UpdateProduct(product);
	      
	    return Ok(result);
    }
}