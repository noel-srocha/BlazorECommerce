using Microsoft.AspNetCore.Mvc;

namespace BlazorECommerce.Server.Controllers;

using Shared.UAC;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    [HttpPost("register")]
    public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegister request)
    {
        var response = await _authService.Register(new User
        { 
            EmailAddress = request.EmailAddress
        }, 
        request.Password);

        if (!response.Success)
            return BadRequest();
        
        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<ActionResult<ServiceResponse<string>>> Login(UserLogin request)
    {
        var response = await _authService.Login(request.EmailAddress, request.Password);
        
        if (!response.Success)
            return BadRequest();

        return Ok(response);
    }
}