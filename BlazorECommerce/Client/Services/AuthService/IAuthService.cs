namespace BlazorECommerce.Client.Services.AuthService;

using BlazorECommerce.Shared.UAC;

public interface IAuthService
{
    Task<ServiceResponse<bool>> ChangePassword(UserChangePassword request);
    Task<ServiceResponse<string>> Login(UserLogin request);
    Task<ServiceResponse<int>> Register(UserRegister request);
    Task<bool> IsUserAuthenticated();
}