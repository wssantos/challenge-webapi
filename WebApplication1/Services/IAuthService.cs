using ProductApi.Models.DTOs;

namespace ProductApi.Services
{
    public interface IAuthService
    {
        Task<LoginResponse?> LoginAsync(LoginRequest request);
        string GenerateJwtToken(string username);
    }
}