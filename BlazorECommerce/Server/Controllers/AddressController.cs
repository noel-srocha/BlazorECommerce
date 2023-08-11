using Microsoft.AspNetCore.Mvc;

namespace BlazorECommerce.Server.Controllers;

using Microsoft.AspNetCore.Authorization;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AddressController : Controller
{
    private readonly IAddressService _addressService;

    public AddressController(IAddressService addressService)
    {
        _addressService = addressService;
    }
    
    [HttpPost]
    public async Task<ActionResult<ServiceResponse<Address>>> AddOrUpdateAddress(Address address)
    {
        return await _addressService.AddOrUpdateAddress(address);
    }

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<Address>>> GetAddress()
    {
        return await _addressService.GetAddress();
    }
}