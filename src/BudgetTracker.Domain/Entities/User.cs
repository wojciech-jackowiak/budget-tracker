using BudgetTracker.Domain.Common;
using BudgetTracker.Domain.Common.Exceptions;
using BudgetTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetTracker.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public UserRole Role { get; private set; }
        public bool IsActive { get; private set; }

        private readonly List<Transaction> _transactions = new();
        public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();

        private readonly List<RefreshToken> _refreshTokens = new();
        public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();

        private User()
        {
            Username = string.Empty;
            Email = string.Empty;
            PasswordHash = string.Empty;
        }

        public static User Create(string username, string email, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new DomainException("Username cannot be empty");

            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
                throw new DomainException("Invalid email format");

            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new DomainException("Password hash cannot be empty");

            return new User
            {
                Username = username,
                Email = email,
                PasswordHash = passwordHash,
                Role = UserRole.User,
                IsActive = true
            };
        }

        public void ChangePassword(string newPasswordHash)
        {
            if (string.IsNullOrWhiteSpace(newPasswordHash))
                throw new DomainException("Password hash cannot be empty");

            PasswordHash = newPasswordHash;
            MarkAsModified();
        }

        public void UpdateEmail(string newEmail)
        {
            if (string.IsNullOrWhiteSpace(newEmail) || !newEmail.Contains("@"))
                throw new DomainException("Invalid email format");

            Email = newEmail;
            MarkAsModified();
        }

        public void Deactivate()
        {
            IsActive = false;
            MarkAsModified();
        }

        public void Activate()
        {
            IsActive = true;
            MarkAsModified();
        }

        public void AddRefreshToken(RefreshToken token)
        {
            _refreshTokens.Add(token);
            MarkAsModified();
        }

        public void RemoveExpiredRefreshTokens()
        {
            var expired = _refreshTokens.Where(t => t.ExpiresAt < DateTime.UtcNow).ToList();
            foreach (var token in expired)
            {
                _refreshTokens.Remove(token);
            }
            if (expired.Any())
                MarkAsModified();
        }
    }
}
