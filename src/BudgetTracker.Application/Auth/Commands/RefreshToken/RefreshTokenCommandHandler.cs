using BudgetTracker.Application.Auth.Common.DTOs;
using BudgetTracker.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetTracker.Application.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResponseDto>
    {
        private readonly IBudgetTrackerDbContext _context;
        private readonly IJwtService _jwtService;

        public RefreshTokenCommandHandler(
            IBudgetTrackerDbContext context,
            IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<AuthResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
             var refreshToken = await _context.RefreshTokens
                 .Include(rt => rt.User)
                 .FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken, cancellationToken);

             if (refreshToken == null)
                throw new UnauthorizedAccessException("Invalid refresh token");
             if (refreshToken.ExpiresAt < DateTime.UtcNow)
                throw new UnauthorizedAccessException("Refresh token expired");
             if (refreshToken.RevokedAt != null)
                throw new UnauthorizedAccessException("Refresh token revoked");
             if (!refreshToken.User.IsActive)
                throw new UnauthorizedAccessException("Account is deactivated");
             refreshToken.Revoke();  

            var (accessToken, accessTokenExpires) = _jwtService.GenerateAccessToken(refreshToken.User);
            var (newRefreshToken, refreshTokenExpires) = await _jwtService.GenerateRefreshTokenAsync(
                refreshToken.User.Id,
                cancellationToken
            );
            refreshToken.MarkAsReplaced(newRefreshToken);
            await _context.SaveChangesAsync(cancellationToken);


            return new AuthResponseDto
            {
                UserId = refreshToken.User.Id,
                Username = refreshToken.User.Username,
                Email = refreshToken.User.Email,
                AccessToken = accessToken,
                RefreshToken = newRefreshToken,
                AccessTokenExpiresAt = accessTokenExpires,
                RefreshTokenExpiresAt = refreshTokenExpires
            };
        }
    }
}
