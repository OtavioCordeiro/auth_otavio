using BattleFace.Application.DTOs;
using BattleFace.Application.Interfaces;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;

namespace BattleFace.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            if (registerRequestDto == null)
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                var authResponse = await _authService.RegisterAsync(registerRequestDto);
                return Ok(authResponse); // Retorna o AuthResponseDto com o token e os dados do usuário
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); // Retorna mensagem de erro
            }
        }

        // POST api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            if (loginRequestDto == null)
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                var authResponse = await _authService.LoginAsync(loginRequestDto);
                return Ok(authResponse); // Retorna o AuthResponseDto com o token e os dados do usuário
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(ex.Message); // Retorna 401 em caso de erro de login
            }
        }

        [HttpPost("google")]
        public async Task<IActionResult> Google([FromBody] GoogleRequestDto request)
        {
            if (request == null)
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                var authResponse = await _authService.GoogleAsync(request);
                return Ok(authResponse); // Retorna o AuthResponseDto com o token e os dados do usuário
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); // Retorna mensagem de erro
            }
        }

        // POST api/auth/forgot-password
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
                return BadRequest("E-mail é obrigatório.");

            var originUrl = Request.Headers["Origin"].ToString();

            await _authService.ForgotPasswordAsync(request.Email, originUrl);

            return Ok(new { message = "Se o e-mail estiver cadastrado, um link de redefinição será enviado." });
        }

        // POST api/auth/reset-password
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Token) || string.IsNullOrWhiteSpace(request.NewPassword))
                return BadRequest("Token e nova senha são obrigatórios.");

            try
            {
                await _authService.ResetPasswordAsync(request);
                return Ok(new { message = "Senha redefinida com sucesso." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
