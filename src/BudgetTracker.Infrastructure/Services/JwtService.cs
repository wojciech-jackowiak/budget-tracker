using BudgetTracker.Application.Common.Interfaces;
using BudgetTracker.Domain.Entities;
using BudgetTracker.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BudgetTracker.Infrastructure.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IBudgetTrackerDbContext _context;

        public JwtService(
            IOptions<JwtSettings> jwtSettings,
            IBudgetTrackerDbContext context)
        {
            _jwtSettings = jwtSettings.Value;
            _context = context;
        }
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);

        }
        public bool VerifyPassword(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
        public (string Token, DateTime ExpiresAt) GenerateAccessToken(User user)
        {
            var claims = new List<Claim>
                    {
                        new Claim("sub", user.Id.ToString()),       // ← KRÓTKI "sub"
                        new Claim("name", user.Username),
                        new Claim("email", user.Email),
                        new Claim("role", user.Role.ToString())
                    };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes);

             var token = new JwtSecurityToken(
                 issuer: _jwtSettings.Issuer,
                 audience: _jwtSettings.Audience,
                 claims: claims,
                 expires: expiresAt,
                 signingCredentials: credentials
             );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return (tokenString, expiresAt);

        }
        public async Task<(string Token, DateTime ExpiresAt)> GenerateRefreshTokenAsync(
            int userId, CancellationToken cancellationToken = default)
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            var tokenString = Convert.ToBase64String(randomBytes);

            var expiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays);

            var refreshToken = RefreshToken.Create(
                userId: userId,
                token: tokenString,
                expiresAt: expiresAt,
                createdByIp: null  // TODO: Get from HttpContext later
            );
            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync(cancellationToken);

            return (tokenString, expiresAt);
        }

    }
}