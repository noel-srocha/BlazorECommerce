using Microsoft.AspNetCore.Mvc;

namespace BlazorECommerce.Server.Controllers;

using Services.CategoryService;

public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet("getcategories")]
    public async Task<IActionResult> GetCategories()
    {
        var result = await _categoryService.GetCategories();

        return Ok(result);
    }
}