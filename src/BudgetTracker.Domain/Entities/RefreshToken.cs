using BudgetTracker.Domain.Common;
using BudgetTracker.Domain.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetTracker.Domain.Entities
{
    public class RefreshToken : BaseEntity
    {


        public string Token { get; private set; }
        public DateTime ExpiresAt { get; private set; }
        public DateTime? RevokedAt { get; private set; }

        public string? ReplacedByToken { get; private set; }
        public string? CreatedByIp { get; private set; }
        public string? RevokedByIp { get; private set; }

        // Navigation property
        public int UserId { get; private set; }
        public User User { get; private set; } = null!;
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
        public bool IsRevoked => RevokedAt != null;
        public bool IsActive => !IsRevoked && !IsExpired;


        private RefreshToken()
        {
            Token = string.Empty;
        }

        #region Factory Methods
        /// <summary>
        /// Tworzy nowy refresh token dla użytkownika.
        /// </summary>
        /// <param name="userId">ID użytkownika</param>
        /// <param name="token">Unikalny token (np. GUID)</param>
        /// <param name="expiresAt">Data wygaśnięcia</param>
        /// <param name="createdByIp">IP z którego utworzono token (audit)</param>
        public static RefreshToken Create(
            int userId,
            string token,
            DateTime expiresAt,
            string? createdByIp = null)
        {
            ValidateToken(token);
            ValidateExpirationDate(expiresAt);

            return new RefreshToken
            {
                UserId = userId,
                Token = token,
                ExpiresAt = expiresAt,
                CreatedByIp = createdByIp
            };
        }

        #endregion

        #region Business Methods

        /// <summary>
        /// Unieważnia token i opcjonalnie oznacza token którym został zastąpiony.
        /// </summary>
        /// <param name="revokedByIp">IP z którego unieważniono (audit)</param>
        /// <param name="replacedByToken">Token którym został zastąpiony (podczas refresh)</param>
        public void Revoke(string? revokedByIp = null, string? replacedByToken = null)
        {
            if (IsRevoked)
                throw new DomainException("Token is already revoked");

            RevokedAt = DateTime.UtcNow;
            RevokedByIp = revokedByIp;
            ReplacedByToken = replacedByToken;
            MarkAsModified();
        }

        /// <summary>
        /// Sprawdza czy token może być użyty do refresh operacji.
        /// </summary>
        public void ValidateForRefresh()
        {
            if (IsExpired)
                throw new DomainException("Refresh token has expired", "TOKEN_EXPIRED");

            if (IsRevoked)
                throw new DomainException("Refresh token has been revoked", "TOKEN_REVOKED");
        }

        /// <summary>
        /// Sprawdza czy token wygasa wkrótce (np. w ciągu 24h).
        /// Przydatne do automatycznego refresh przed wygaśnięciem.
        /// </summary>
        public bool IsExpiringSoon(TimeSpan threshold)
        {
            return ExpiresAt - DateTime.UtcNow <= threshold;
        }
        #endregion

        #region Validation
        private static void ValidateToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new DomainException("Token cannot be empty", "TOKEN_EMPTY");

            // Opcjonalnie: sprawdź format tokena
            if (token.Length < 32)
                throw new DomainException("Token is too short (minimum 32 characters)", "TOKEN_TOO_SHORT");
        }

        private static void ValidateExpirationDate(DateTime expiresAt)
        {
            if (expiresAt <= DateTime.UtcNow)
                throw new DomainException(
                    "Token expiration date must be in the future",
                    "TOKEN_INVALID_EXPIRATION");
        }

        #endregion
    }
}
