using BudgetTracker.Application.Auth.Common.DTOs;
using BudgetTracker.Application.Common.Interfaces;
using BudgetTracker.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using BudgetTracker.Application.Common.Exceptions;

namespace BudgetTracker.Application.Auth.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponseDto>
    {
        private readonly IBudgetTrackerDbContext _context;
        private readonly IJwtService _jwtService;  // Zaimplementujemy później!

        public RegisterCommandHandler(
            IBudgetTrackerDbContext context,
            IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<AuthResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var usernameExists = await _context.Users
                .AnyAsync(u => u.Username == request.Username, cancellationToken);

            if (usernameExists)
            {
                var errors = new Dictionary<string, string[]>
            {
                { "Username", new[] { "Username is already taken" } }
            };
                throw new ValidationException(errors);
            }

            var emailExists = await _context.Users
                .AnyAsync(u => u.Email == request.Email, cancellationToken);

            if (emailExists)
            {
                var errors = new Dictionary<string, string[]>
            {
                { "Email", new[] { "Email is already registered" } }
            };
                throw new ValidationException(errors);
            }
            var passwordHash = _jwtService.HashPassword(request.Password);


            var user = User.Create(
                username: request.Username,
                email: request.Email,
                passwordHash: passwordHash
            );

            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);

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
