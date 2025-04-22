using BattleFace.Application.DTOs;
using BattleFace.Application.Interfaces;
using BattleFace.Domain.Entities;
using BattleFace.Domain.Interfaces;
using BattleFace.Application.Helpers;
using AutoMapper;
using System;
using System.Threading.Tasks;
using Google.Apis.Auth;

namespace BattleFace.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public AuthService(IUserRepository userRepository, ITokenService tokenService, IMapper mapper, IEmailService emailService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _mapper = mapper;
            _emailService = emailService;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto registerRequestDto)
        {
            if (registerRequestDto.Password != registerRequestDto.ConfirmPassword)
            {
                throw new InvalidOperationException("Password and confirm password do not match.");
            }

            var existingUser = await _userRepository.GetByEmailAsync(registerRequestDto.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Email is already in use.");
            }

            var user = _mapper.Map<User>(registerRequestDto);

            user.UserRoles.Add(new UserRole
            {
                RoleId = 1
            });
            user.PlanId = 1;

            await _userRepository.AddAsync(user);

            var userDto = _mapper.Map<UserDto>(user);

            var token = _tokenService.GenerateToken(userDto);

            return new AuthResponseDto
            {
                Token = token,
                User = userDto
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto loginRequestDto)
        {
            // Validate user credentials
            var user = await _userRepository.GetByEmailAsync(loginRequestDto.Email);
            if (user == null || !PasswordHasher.VerifyPassword(loginRequestDto.Password, user.Credential.PasswordHash))
            {
                throw new InvalidOperationException("Invalid credentials.");
            }

            var userDto = _mapper.Map<UserDto>(user);

            var token = _tokenService.GenerateToken(userDto);

            return new AuthResponseDto
            {
                Token = token,
                User = userDto
            };
        }

        public async Task<AuthResponseDto> GoogleAsync(GoogleRequestDto request)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(request.Token);

            var user = await _userRepository.GetByEmailAsync(payload.Email);
            if (user == null)
            {
                User newUser = new User
                {
                    Email = payload.Email,
                    Name = payload.Name,
                    ExternalAuthProviders = new List<ExternalAuthProvider>
                    {
                        new ExternalAuthProvider
                        {
                            AuthProviderId = 1,
                            ProviderUserId = payload.Subject
                        }
                    }
                };

                await _userRepository.AddAsync(newUser);
            }

            var userDto = _mapper.Map<UserDto>(user);

            var token = _tokenService.GenerateToken(userDto);

            return new AuthResponseDto
            {
                Token = token,
                User = userDto
            };
        }

        public async Task ForgotPasswordAsync(string email, string originUrl)
        {
            var user = await _userRepository.GetByEmailAsync(email);

            var expireInMinutes = 5;

            if (user?.Credential == null)
                return;

            var token = Guid.NewGuid().ToString();
            var expiresAt = DateTime.UtcNow.AddMinutes(expireInMinutes);

            var recovery = new PasswordRecovery
            {
                UserId = user.Id,
                Token = token,
                ExpiresAt = expiresAt,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.AddPasswordRecoveryAsync(recovery);

            var resetLink = $"{originUrl}/reset-password?token={token}";

            var body = $@"
                            <h2>Password Reset Request</h2>
                            <p>To reset your password, click the link below:</p>
                            <a href='{resetLink}' style='padding: 10px 20px; background-color: #007bff; color: white; text-decoration: none; border-radius: 5px;'>Reset Password</a>
                            <p>The link will expire in {expireInMinutes} minutes.</p>";

            // Envia o e-mail com o link de reset
            await _emailService.SendAsync(user.Email, "Password Reset", body);
        }

        public async Task ResetPasswordAsync(ResetPasswordRequestDto request)
        {
            var recovery = await _userRepository.GetValidPasswordRecoveryAsync(request.Token);

            if (recovery == null)
                throw new Exception("Invalid or expired token.");

            var hashedPassword = PasswordHasher.HashPassword(request.NewPassword);

            recovery.User.Credential.PasswordHash = hashedPassword;
            await _userRepository.MarkPasswordRecoveryAsUsedAsync(recovery.Id);
        }
    }
}
