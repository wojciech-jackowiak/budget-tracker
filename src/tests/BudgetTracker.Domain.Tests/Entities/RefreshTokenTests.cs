using BudgetTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;

namespace BudgetTracker.Domain.Tests.Entities
{
    public class RefreshTokenTests
    {
        #region IsExpired Tests
        [Fact]
        public void IsExpired_WhenExpiresAtIsInPast_ShouldReturnTrue()
        {
            // Arrange
            var token = new RefreshToken
            {
                ExpiresAt = DateTime.UtcNow.AddDays(-1)
            };

            // Act
            var result = token.IsExpired;

            // Assert
            result.Should().BeTrue();
        }
        [Fact]
        public void IsExpired_WhenExpiresAtIsInFuture_ShouldReturnFalse()
        {
            // Arrange
            var token = new RefreshToken
            {
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };

            // Act
            var result = token.IsExpired;

            // Assert
            result.Should().BeFalse();
        }
        [Fact]
        public void IsExpired_WhenExpiresAtIsNow_ShouldReturnTrue()
        {
            // Arrange
            var token = new RefreshToken
            {
                ExpiresAt = DateTime.UtcNow
            };

            // Act
            var result = token.IsExpired;

            // Assert
            result.Should().BeTrue();
        }
        #endregion
        #region IsRevoked Tests
        [Fact]
        public void IsRevoked_WhenRevokedAtIsNull_ShouldReturnFalse()
        {
            // Arrange
            var token = new RefreshToken
            {
                RevokedAt = null
            };

            // Act
            var result = token.IsRevoked;

            // Assert
            result.Should().BeFalse();
        }
        [Fact]
        public void IsRevoked_WhenRevokedAtHasValue_ShouldReturnTrue()
        {
            // Arrange
            var token = new RefreshToken
            {
                RevokedAt = DateTime.UtcNow
            };

            // Act
            var result = token.IsRevoked;

            // Assert
            result.Should().BeTrue();
        }
        [Fact]
        public void IsActive_WhenNotExpiredAndNotRevoked_ShouldReturnTrue()
        {
            // Arrange
            var token = new RefreshToken
            {
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                RevokedAt = null
            };

            // Act
            var result = token.IsActive;

            // Assert
            result.Should().BeTrue();
        }
        [Fact]
        public void IsActive_WhenExpired_ShouldReturnFalse()
        {
            // Arrange
            var token = new RefreshToken
            {
                ExpiresAt = DateTime.UtcNow.AddDays(-1),
                RevokedAt = null
            };

            // Act
            var result = token.IsActive;

            // Assert
            result.Should().BeFalse();
        }
        [Fact]
        public void IsActive_WhenExpiredAndRevoked_ShouldReturnFalse()
        {
            // Arrange
            var token = new RefreshToken
            {
                ExpiresAt = DateTime.UtcNow.AddDays(-1),
                RevokedAt = DateTime.UtcNow.AddDays(-2)
            };

            // Act
            var result = token.IsActive;

            // Assert
            result.Should().BeFalse();
        }
        #endregion
    }
}
