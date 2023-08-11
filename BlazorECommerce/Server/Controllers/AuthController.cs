using Microsoft.AspNetCore.Mvc;

namespace BlazorECommerce.Server.Controllers;

using Microsoft.AspNetCore.Authorization;
using Shared.UAC;
using System.Security.Claims;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    [HttpPost("change-password"), Authorize]
    public async Task<ActionResult<ServiceResponse<bool>>> ChangePassword([FromBody] string newPassword)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var response = await _authService.ChangePassword(int.Parse(userId!), newPassword);

        if (!response.Success)
            return BadRequest(response);

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
}