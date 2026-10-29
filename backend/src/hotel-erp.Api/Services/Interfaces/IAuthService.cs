using hotel_erp.Api.Dtos.Auth;
using hotel_erp.Api.Dtos.Common;

namespace hotel_erp.Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request);
        Task LogoutAsync(Guid userId, int securityVersion);
        Task ChangePasswordAsync(Guid userId, ChangePasswordRequest request, int securityVersion);
    }

    public interface IJwtService
    {
        Task<AuthResponse> GenerateTokensAsync(Guid userId);
        Task<Guid?> ConsumeRefreshTokenAsync(string refreshToken);
        Task RevokeAllRefreshTokensAsync(Guid userId);
    }
}


