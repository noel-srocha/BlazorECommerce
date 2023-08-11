using Microsoft.AspNetCore.Mvc;

namespace BlazorECommerce.Server.Controllers;

using Microsoft.AspNetCore.Authorization;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class ProductTypeController : ControllerBase
{
    private readonly IProductTypeService _productTypeService;

    public ProductTypeController(IProductTypeService productTypeService)
    {
        _productTypeService = productTypeService;
    }
    
    [HttpPost]
    public async Task<ActionResult<List<ServiceResponse<List<ProductType>>>>> AddProductType(ProductType productType)
    {
        var result = await _productTypeService.AddProductType(productType);

        return Ok(result);
    }
    
    [HttpGet]
    public async Task<ActionResult<List<ServiceResponse<List<ProductType>>>>> GetProductTypes()
    {
        var result = await _productTypeService.GetProductTypes();

        return Ok(result);
    }
    
    [HttpPut]
    public async Task<ActionResult<List<ServiceResponse<List<ProductType>>>>> UpdateProductType(ProductType productType)
    {
        var result = await _productTypeService.UpdateProductType(productType);

        return Ok(result);
    }
}