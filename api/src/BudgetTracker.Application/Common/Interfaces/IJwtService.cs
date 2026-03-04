using BudgetTracker.Domain.Entities;

namespace BudgetTracker.Application.Common.Interfaces
{
    public interface IJwtService
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string passwordHash);
        (string Token, DateTime ExpiresAt) GenerateAccessToken(User user);
         Task<(string Token, DateTime ExpiresAt)> GenerateRefreshTokenAsync(
             int userId,
             CancellationToken cancellationToken = default
         );
    }
}