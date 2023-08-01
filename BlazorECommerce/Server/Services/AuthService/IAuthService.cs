namespace BlazorECommerce.Server.Services.AuthService;

using Shared.UAC;

public interface IAuthService
{
    Task<ServiceResponse<int>> Register(User user, string password);
    Task<bool> UserExists(string emailAddress);
    Task<ServiceResponse<string>> Login(string emailAddress, string password);
    Task<ServiceResponse<bool>> ChangePassword(int userId, string newPassword);
}