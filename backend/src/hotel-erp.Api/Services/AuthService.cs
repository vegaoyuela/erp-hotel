using hotel_erp.Api.Dtos.Auth;
using hotel_erp.Api.Database.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using hotel_erp.Api.Dtos.Common;
using hotel_erp.Api.Services.Interfaces;
using hotel_erp.Api.Database.Entities;
using hotel_erp.Api.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using hotel_erp.Api.Authorization;

namespace hotel_erp.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _context;
        private readonly IHotelClock _clock;
        private readonly IAuditService _audit;
        private readonly IApplicationFailureInjector _failureInjector;

        public AuthService(
            IUserRepository userRepository,
            IJwtService jwtService,
            ApplicationDbContext context,
            IHotelClock clock, IUnitOfWork unitOfWork, IAuditService audit, IApplicationFailureInjector failureInjector)
        {
            _failureInjector = failureInjector;
            _audit = audit;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _jwtService = jwtService;
            _context = context;
            _clock = clock;
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var userId = await _context.Users.AsNoTracking().Where(row => row.Username == request.Username)
                .Select(row => (Guid?)row.Id).SingleOrDefaultAsync();
            if (userId is null) return new AuthResponse { Success = false, Message = "Credenciales inválidas" };
            await using var transaction = await SecurityMutationLocks.BeginSessionAsync(_context, userId.Value);
            var user = await _userRepository.GetByUsernameAsync(request.Username);
            if (user == null)
                return new AuthResponse { Success = false, Message = "Credenciales inválidas" };

            if (user.LockoutEnd.HasValue && user.LockoutEnd > _clock.UtcNow)
                return new AuthResponse { Success = false, Message = "Credenciales inválidas" };

            if (!user.IsActive || user.IsDeleted)
                return new AuthResponse { Success = false, Message = "Credenciales inválidas" };

            if (!IsValidPassword(request.Password, user.PasswordHash))
            {
                user.FailedLoginAttempts++;
                if (user.FailedLoginAttempts >= 5)
                    user.LockoutEnd = _clock.UtcNow.AddMinutes(15);
                await _unitOfWork.SaveChangesAsync();
                await transaction.CommitAsync();
                return new AuthResponse { Success = false, Message = "Credenciales inválidas" };
            }

            user.FailedLoginAttempts = 0;
            user.LockoutEnd = null;
            user.LastLogin = _clock.UtcNow;
            await _unitOfWork.SaveChangesAsync();

            await _failureInjector.CheckpointAsync(ApplicationFailureCheckpoints.SecurityAfterPersistence);
            var session = await _jwtService.GenerateTokensAsync(user.Id);
            await _failureInjector.CheckpointAsync(ApplicationFailureCheckpoints.SecurityAfterRevocation);
            await _audit.LogAsync(user.Id, "Login", null, null);
            await _failureInjector.CheckpointAsync(ApplicationFailureCheckpoints.SecurityAfterAudit);
            await transaction.CommitAsync();
            return session;
        }

        private static bool IsValidPassword(string password, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
                return false;

            try
            {
                return BCrypt.Net.BCrypt.Verify(password, passwordHash);
            }
            catch
            {
                return false;
            }
        }

        public async Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.RefreshToken))
                return new AuthResponse { Success = false, Message = "Token inválido o expirado" };

            var userId = await _context.RefreshTokens.AsNoTracking()
                .Where(row => row.Token == JwtService.HashToken(request.RefreshToken))
                .Select(row => (Guid?)row.UserId).SingleOrDefaultAsync();
            if (userId is null) return new AuthResponse { Success = false, Message = "Token inválido o expirado" };
            await using var transaction = await SecurityMutationLocks.BeginSessionAsync(_context, userId.Value);
            var consumedUserId = await _jwtService.ConsumeRefreshTokenAsync(request.RefreshToken);
            await _failureInjector.CheckpointAsync(ApplicationFailureCheckpoints.SecurityAfterPersistence);
            var result = consumedUserId is null
                ? new AuthResponse { Success = false, Message = "Token inválido o expirado" }
                : await _jwtService.GenerateTokensAsync(consumedUserId.Value);
            await _failureInjector.CheckpointAsync(ApplicationFailureCheckpoints.SecurityAfterRevocation);
            await _audit.LogAsync(userId.Value, consumedUserId is null ? "RefreshRejected" : "RefreshSession", nameof(User), userId.Value);
            await _failureInjector.CheckpointAsync(ApplicationFailureCheckpoints.SecurityAfterAudit);
            // Replay detection may revoke the entire family even when refresh fails.
            await transaction.CommitAsync();
            return result;
        }

        public async Task LogoutAsync(Guid userId, int securityVersion)
        {
            await using var transaction = await SecurityMutationLocks.BeginSessionAsync(_context, userId);
            var user = await _context.Users.FirstOrDefaultAsync(candidate => candidate.Id == userId);
            if (user is null || !user.IsActive || user.IsDeleted || user.SecurityVersion != securityVersion)
                throw new SecuritySessionRevokedException();
            if (user is not null)
            {
                user.SecurityVersion++;
                await _context.SaveChangesAsync();
            }

            await _failureInjector.CheckpointAsync(ApplicationFailureCheckpoints.SecurityAfterPersistence);
            await _jwtService.RevokeAllRefreshTokensAsync(userId);
            await _failureInjector.CheckpointAsync(ApplicationFailureCheckpoints.SecurityAfterRevocation);
            await _audit.LogAsync(userId, "Logout", null, userId);
            await _failureInjector.CheckpointAsync(ApplicationFailureCheckpoints.SecurityAfterAudit);
            await transaction.CommitAsync();
        }

        public async Task ChangePasswordAsync(Guid userId, ChangePasswordRequest request, int securityVersion)
        {
            await using var transaction = await SecurityMutationLocks.BeginSessionAsync(_context, userId);
            var user = await _userRepository.GetByIdAsync(userId);
            if (user is null || !user.IsActive || user.IsDeleted || user.SecurityVersion != securityVersion)
                throw new SecuritySessionRevokedException();

            if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash))
                throw new UnauthorizedAccessException("Contraseña actual incorrecta");

            if (BCrypt.Net.BCrypt.Verify(request.NewPassword, user.PasswordHash))
                throw new PasswordChangeRejectedException("La contraseña nueva debe ser diferente de la contraseña actual");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            user.MustChangePassword = false;
            user.SecurityVersion++;
            await _unitOfWork.SaveChangesAsync();
            await _failureInjector.CheckpointAsync(ApplicationFailureCheckpoints.SecurityAfterPersistence);
            await _jwtService.RevokeAllRefreshTokensAsync(userId);
            await _failureInjector.CheckpointAsync(ApplicationFailureCheckpoints.SecurityAfterRevocation);
            await _audit.LogAsync(userId, "ChangePassword", nameof(User), userId);
            await _failureInjector.CheckpointAsync(ApplicationFailureCheckpoints.SecurityAfterAudit);
            await transaction.CommitAsync();
        }

    }

    public class JwtService : IJwtService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly IHotelClock _clock;

        public JwtService(IUserRepository userRepository, IMapper mapper, IConfiguration configuration, ApplicationDbContext context, IHotelClock clock)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _configuration = configuration;
            _context = context;
            _clock = clock;
        }

        public async Task<AuthResponse> GenerateTokensAsync(Guid userId)
        {
            RequireTransaction();
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null || !user.IsActive || user.IsDeleted)
                return new AuthResponse { Success = false, Message = "Usuario no disponible" };

            var jwtSettings = _configuration.GetSection("Jwt");
            var secretKey = jwtSettings["SecretKey"]!;
            var expirationMinutes = int.Parse(jwtSettings["ExpirationInMinutes"] ?? "60");

            var roles = await _userRepository.GetUserRolesAsync(userId);
            var permissions = await _userRepository.GetUserPermissionsAsync(userId);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.Username),
                new(ClaimTypes.Email, user.Email),
                new("fullName", $"{user.FirstName} {user.LastName}"),
                new(SecurityClaimTypes.SecurityVersion, user.SecurityVersion.ToString(System.Globalization.CultureInfo.InvariantCulture)),
                new(SecurityClaimTypes.MustChangePassword, user.MustChangePassword.ToString())
            };

            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r.Name)));
            claims.AddRange(permissions.Select(p => new Claim(SecurityClaimTypes.Permission, p.Name)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = _clock.UtcNow.AddMinutes(expirationMinutes);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            var refreshToken = GenerateRefreshToken();
            var refreshTokenHash = HashToken(refreshToken);
            var refreshExpirationDays = int.Parse(jwtSettings["RefreshTokenExpirationInDays"] ?? "7");

            await RevokeAllRefreshTokensAsync(userId);
            await _context.RefreshTokens.AddAsync(new RefreshToken
            {
                UserId = userId,
                Token = refreshTokenHash,
                ExpiresAt = _clock.UtcNow.AddDays(refreshExpirationDays),
                IsRevoked = false
            });
            await _context.SaveChangesAsync();

            var userDto = _mapper.Map<UserDto>(user);
            userDto.Roles = roles.Select(r => r.Name).ToList();
            userDto.Permissions = permissions.Select(p => p.Name).ToList();

            return new AuthResponse
            {
                Success = true,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = expires,
                User = userDto
            };
        }

        public async Task<Guid?> ConsumeRefreshTokenAsync(string refreshToken)
        {
            RequireTransaction();
            var tokenHash = HashToken(refreshToken);
            var token = await _context.RefreshTokens
                .Include(refreshTokenEntity => refreshTokenEntity.User)
                .FirstOrDefaultAsync(refreshTokenEntity => refreshTokenEntity.Token == tokenHash);

            if (token is null
                || token.ExpiresAt <= _clock.UtcNow
                || !token.User.IsActive
                || token.User.IsDeleted)
                return null;

            if (token.IsRevoked)
            {
                await RevokeTokenFamilyAsync(token.User);
                return null;
            }

            var consumedAt = _clock.UtcNow;
            var affected = await _context.RefreshTokens
                .Where(refreshTokenEntity => refreshTokenEntity.Id == token.Id && !refreshTokenEntity.IsRevoked)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(refreshTokenEntity => refreshTokenEntity.IsRevoked, true)
                    .SetProperty(refreshTokenEntity => refreshTokenEntity.RevokedAt, consumedAt));

            if (affected == 0)
            {
                await RevokeTokenFamilyAsync(token.User);
                return null;
            }

            return token.UserId;
        }

        public async Task RevokeAllRefreshTokensAsync(Guid userId)
        {
            RequireTransaction();
            var activeTokens = await _context.RefreshTokens
                .Where(rt => rt.UserId == userId && !rt.IsRevoked)
                .ToListAsync();

            if (activeTokens.Count == 0) return;

            foreach (var token in activeTokens)
            {
                token.IsRevoked = true;
                token.RevokedAt = _clock.UtcNow;
            }

            await _context.SaveChangesAsync();
        }

        private async Task RevokeTokenFamilyAsync(User user)
        {
            user.SecurityVersion++;
            await _context.RefreshTokens
                .Where(refreshTokenEntity => refreshTokenEntity.UserId == user.Id && !refreshTokenEntity.IsRevoked)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(refreshTokenEntity => refreshTokenEntity.IsRevoked, true)
                    .SetProperty(refreshTokenEntity => refreshTokenEntity.RevokedAt, _clock.UtcNow));
            await _context.SaveChangesAsync();
        }

        private void RequireTransaction()
        {
            if (_context.Database.CurrentTransaction is null)
                throw new InvalidOperationException("Las sesiones deben modificarse dentro de una transacción coordinada de seguridad");
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber)
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');
        }

        internal static string HashToken(string token)
            => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)));
    }
}
