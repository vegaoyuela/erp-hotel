using hotel_erp.Api.Dtos.Auth;
using hotel_erp.Api.Dtos.Common;
using hotel_erp.Api.Services.Interfaces;
using hotel_erp.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using hotel_erp.Api.Authorization;
using Microsoft.AspNetCore.RateLimiting;

namespace hotel_erp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly AuthenticationSessionCookies _sessionCookies;
        private readonly SecurityAdministrationService _security;

        public AuthController(
            IAuthService authService,
            AuthenticationSessionCookies sessionCookies, SecurityAdministrationService security)
        {
            _authService = authService;
            _sessionCookies = sessionCookies;
            _security = security;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [EnableRateLimiting(AuthenticationRateLimitPolicyNames.Login)]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
        {
            Response.Headers.CacheControl = "no-store";
            var result = await _authService.LoginAsync(request);
            if (!result.Success)
            {
                return Problem(
                    type: "https://httpstatuses.com/401",
                    title: "Autenticación rechazada",
                    statusCode: StatusCodes.Status401Unauthorized,
                    detail: "Usuario o contraseña inválidos.");
            }
            _sessionCookies.Issue(Response, result.RefreshToken!);
            result.RefreshToken = null;
            return Ok(result);
        }

        [Authorize(Policy = PermissionNames.ManageUsers)]
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
        {
            var result = await _security.CreateUserAsync(SecurityActor.From(User), new CreateUserRequest(
                request.Username, request.Password, request.Email, request.FirstName, request.LastName, null),
                HttpContext.RequestAborted, receptionRegistration: true);
            return result.Succeeded ? Ok(new AuthResponse { Success = true, User = result.Value, Message = "Usuario registrado" })
                : StatusCode(result.FailureStatus!.Value, result.FailureDetail);
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        [EnableRateLimiting(AuthenticationRateLimitPolicyNames.Refresh)]
        public async Task<ActionResult<AuthResponse>> Refresh([FromBody] RefreshTokenRequest request)
        {
            Response.Headers.CacheControl = "no-store";
            var refreshToken = request.RefreshToken;
            var usesCookie = string.IsNullOrWhiteSpace(refreshToken);
            if (usesCookie)
            {
                refreshToken = _sessionCookies.ReadRefreshToken(Request);
                if (string.IsNullOrWhiteSpace(refreshToken))
                {
                    return SessionRejected();
                }

                if (!_sessionCookies.HasValidCsrfToken(Request))
                {
                    return Problem(
                        type: "https://httpstatuses.com/403",
                        title: "Solicitud de sesión rechazada",
                        statusCode: StatusCodes.Status403Forbidden,
                        detail: "La comprobación CSRF no es válida.");
                }
            }

            var result = await _authService.RefreshTokenAsync(new RefreshTokenRequest(refreshToken));
            if (!result.Success || string.IsNullOrWhiteSpace(result.RefreshToken))
            {
                _sessionCookies.Clear(Response);
                return SessionRejected();
            }

            _sessionCookies.Issue(Response, result.RefreshToken);
            result.RefreshToken = null;
            return Ok(result);
        }

        [Authorize]
        [HttpPost("logout")]
        [ProducesResponseType(typeof(MessageResponseDto), StatusCodes.Status200OK)]
        public async Task<ActionResult> Logout()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            try
            {
                await _authService.LogoutAsync(userId, SecurityActor.From(User).Version);
                return Ok(new { message = "Sesión cerrada exitosamente" });
            }
            catch (SecuritySessionRevokedException)
            {
                return SessionRejected();
            }
            finally
            {
                // El navegador no debe conservar una cookie utilizable si la revocación
                // o su auditoría fallan por una interrupción de base de datos.
                _sessionCookies.Clear(Response);
            }
        }

        [Authorize]
        [HttpPost("change-password")]
        [EnableRateLimiting(AuthenticationRateLimitPolicyNames.ChangePassword)]
        [ProducesResponseType(typeof(MessageResponseDto), StatusCodes.Status200OK)]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            try
            {
                await _authService.ChangePasswordAsync(userId, request, SecurityActor.From(User).Version);
            }
            catch (SecuritySessionRevokedException)
            {
                _sessionCookies.Clear(Response);
                return SessionRejected();
            }
            catch (UnauthorizedAccessException exception)
            {
                return Problem(
                    type: "https://httpstatuses.com/400",
                    title: "No se pudo cambiar la contraseña",
                    statusCode: StatusCodes.Status400BadRequest,
                    detail: exception.Message);
            }
            catch (PasswordChangeRejectedException exception)
            {
                return Problem(
                    type: "https://httpstatuses.com/400",
                    title: "No se pudo cambiar la contraseña",
                    statusCode: StatusCodes.Status400BadRequest,
                    detail: exception.Message);
            }

            _sessionCookies.Clear(Response);
            return Ok(new { message = "Contraseña cambiada exitosamente" });
        }

        [HttpGet("me")]
        [Authorize]
        [ProducesResponseType(typeof(CurrentUserDto), StatusCodes.Status200OK)]
        public ActionResult GetCurrentUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var username = User.FindFirstValue(ClaimTypes.Name);
            var email = User.FindFirstValue(ClaimTypes.Email);
            var fullName = User.FindFirstValue("fullName");
            var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
            var permissions = User.FindAll("permission").Select(c => c.Value).ToList();
            var mustChangePassword = bool.TryParse(
                User.FindFirstValue(SecurityClaimTypes.MustChangePassword),
                out var changeRequired) && changeRequired;

            return Ok(new
            {
                Id = userId,
                Username = username,
                Email = email,
                FullName = fullName,
                Roles = roles,
                Permissions = permissions,
                MustChangePassword = mustChangePassword
            });
        }

        private ObjectResult SessionRejected()
            => Problem(
                type: "https://httpstatuses.com/401",
                title: "Sesión rechazada",
                statusCode: StatusCodes.Status401Unauthorized,
                detail: "La sesión no es válida o ya expiró.");
    }
}
