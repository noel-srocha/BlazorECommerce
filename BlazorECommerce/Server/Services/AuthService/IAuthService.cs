﻿namespace BlazorECommerce.Server.Services.AuthService;

using Shared.UAC;

public interface IAuthService
{
    Task<ServiceResponse<bool>> ChangePassword(int userId, string newPassword);
    Task<User> GetUserByEmail(string emailAddress);
    string GetUserEmail();
    int GetUserId();
    Task<ServiceResponse<string>> Login(string emailAddress, string password);
    Task<ServiceResponse<int>> Register(User user, string password);
    Task<bool> UserExists(string emailAddress);
}