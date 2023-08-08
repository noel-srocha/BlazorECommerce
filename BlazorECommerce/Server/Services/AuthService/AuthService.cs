namespace BlazorECommerce.Server.Services.AuthService;

using Microsoft.IdentityModel.Tokens;
using Shared.UAC;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

public class AuthService : IAuthService
{
    private readonly DataContext _context;
    private readonly IConfiguration _configuration;

    private readonly HttpContextAccessor _httpContextAccessor;

    public AuthService(DataContext context, IConfiguration configuration, HttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<ServiceResponse<bool>> ChangePassword(int userId, string newPassword)
    {
        var user = await _context.Users.FindAsync(userId);
        
        if (user == null)
            return new ServiceResponse<bool> { Success = false, Message = "User not found." };
        
        CreatePasswordHash(newPassword, out var passwordHash, out var passwordSalt);
        
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;

        await _context.SaveChangesAsync();
        
        return new ServiceResponse<bool> { Data = true, Message = "Password changed successfully." };
    }
    public async Task<User> GetUserByEmail(string emailAddress)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.EmailAddress.ToLower().Equals(emailAddress.ToLower()));
    }
    public string GetUserEmail() => _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Name);

    public int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    
    public async Task<ServiceResponse<string>> Login(string emailAddress, string password)
    {
        var response = new ServiceResponse<string>();
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.EmailAddress.ToLower().Equals(emailAddress.ToLower()));

        if (user == null)
        {
            response.Success = false;
            response.Message = "User not found.";
        }
        else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
        {
            response.Success = false;
            response.Message = "Wrong password.";
        }
        else
        {
            response.Data = CreateToken(user);
        }


        return response;
    }
    
    public async Task<ServiceResponse<int>> Register(User user, string password)
    {
        var userExists = await UserExists(user.EmailAddress);
        
        if (userExists)
            return new ServiceResponse<int>() { Success = false, Message = "User already exists" };
        
        CreatePasswordHash(password, out var passwordHash, out var passwordSalt);
        
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        
        return new ServiceResponse<int>() { Data = user.Id, Message = "User created successfully" };
    }
    
    public async Task<bool> UserExists(string emailAddress)
    {
        var emailExists = await _context.Users.AnyAsync(x => x.EmailAddress.ToLower().Equals(emailAddress.ToLower()));

        return emailExists;
    }
    
    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    private string CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.EmailAddress),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(_configuration.GetSection("AppSettings:Token").Value!));
        
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds
        );
        
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
    
    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512(passwordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        
        return computedHash.SequenceEqual(passwordHash);
    }
}