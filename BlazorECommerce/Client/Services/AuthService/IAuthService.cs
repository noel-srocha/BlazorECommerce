namespace BlazorECommerce.Client.Services.AuthService;

using BlazorECommerce.Shared.UAC;

public interface IAuthService
{
    Task<ServiceResponse<int>> Register(UserRegister request);
    Task<ServiceResponse<string>> Login(UserLogin request);
}