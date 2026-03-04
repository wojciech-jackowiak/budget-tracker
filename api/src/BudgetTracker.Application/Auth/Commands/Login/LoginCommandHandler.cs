using BudgetTracker.Application.Auth.Common.DTOs;
using BudgetTracker.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetTracker.Application.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponseDto>
    {
        private readonly IBudgetTrackerDbContext _context;
        private readonly IJwtService _jwtService;

        public LoginCommandHandler(
            IBudgetTrackerDbContext context,
            IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

            if (user == null)
                throw new UnauthorizedAccessException("Invalid email or password");

            var passwordValid = _jwtService.VerifyPassword(request.Password, user.PasswordHash);

            if (!passwordValid)
                throw new UnauthorizedAccessException("Invalid email or password");
            if (!user.IsActive)
                throw new UnauthorizedAccessException("Account is deactivated");

            var (accessToken, accessTokenExpires) = _jwtService.GenerateAccessToken(user);
            var (refreshToken, refreshTokenExpires) = await _jwtService.GenerateRefreshTokenAsync(
                user.Id,
                cancellationToken
            );
            return new AuthResponseDto
            {
                UserId = user.Id,
                Username = user.Username,
                Email = user.Email,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiresAt = accessTokenExpires,
                RefreshTokenExpiresAt = refreshTokenExpires
            };
        }
    }
}
