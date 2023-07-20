using Microsoft.AspNetCore.Mvc;

namespace BlazorECommerce.Server.Controllers;

public class ProductController : Controller
{
  // GET
  public IActionResult Index()
  {
    return View();
  }
}