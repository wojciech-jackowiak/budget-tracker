using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetTracker.Application.Auth.Common.DTOs
{
    public record AuthResponseDto
    {
         public int UserId { get; init; }
        public string Username { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
         public string AccessToken { get; init; } = string.Empty;
        public string RefreshToken { get; init; } = string.Empty;
        public DateTime AccessTokenExpiresAt { get; init; }
        public DateTime RefreshTokenExpiresAt { get; init; }
    }
}