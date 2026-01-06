using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using GuardiaoDasMemorias.DTOs.Auth;
using GuardiaoDasMemorias.Models;
using GuardiaoDasMemorias.Services;

namespace GuardiaoDasMemorias.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ITokenService tokenService,
        ILogger<AuthController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _logger = logger;
    }

    /// <summary>
    /// Registra um novo usuário
    /// </summary>
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            // Verificar se o usuário já existe
            var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                return BadRequest(new { message = "Email já está em uso" });
            }

            // Criar novo usuário
            var user = new ApplicationUser
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                EmailConfirmed = true // Para simplificar, confirmar email automaticamente
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return BadRequest(new { message = "Falha ao criar usuário", errors });
            }

            // Gerar token JWT
            var token = _tokenService.GenerateToken(user);

            var response = new AuthResponseDto
            {
                Token = token,
                Email = user.Email!,
                UserId = user.Id,
                UserName = user.UserName,
                ExpiresAt = DateTime.UtcNow.AddMinutes(60)
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao registrar usuário");
            return StatusCode(500, new { message = "Erro interno do servidor" });
        }
    }

    /// <summary>
    /// Realiza login do usuário
    /// </summary>
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return Unauthorized(new { message = "Email ou senha inválidos" });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                return Unauthorized(new { message = "Email ou senha inválidos" });
            }

            // Gerar token JWT
            var token = _tokenService.GenerateToken(user);

            var response = new AuthResponseDto
            {
                Token = token,
                Email = user.Email!,
                UserId = user.Id,
                UserName = user.UserName,
                ExpiresAt = DateTime.UtcNow.AddMinutes(60)
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao realizar login");
            return StatusCode(500, new { message = "Erro interno do servidor" });
        }
    }

    /// <summary>
    /// Obtém informações do usuário autenticado
    /// </summary>
    [HttpGet("me")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public async Task<ActionResult> GetCurrentUser()
    {
        try
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "Usuário não autenticado" });
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { message = "Usuário não encontrado" });
            }

            return Ok(new
            {
                userId = user.Id,
                email = user.Email,
                userName = user.UserName,
                phoneNumber = user.PhoneNumber,
                createdAt = user.CreatedAt
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar informações do usuário");
            return StatusCode(500, new { message = "Erro interno do servidor" });
        }
    }

    /// <summary>
    /// Realiza logout do usuário
    /// </summary>
    [HttpPost("logout")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public async Task<IActionResult> Logout()
    {
        try
        {
            await _signInManager.SignOutAsync();
            return Ok(new { message = "Logout realizado com sucesso" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao realizar logout");
            return StatusCode(500, new { message = "Erro interno do servidor" });
        }
    }
}
