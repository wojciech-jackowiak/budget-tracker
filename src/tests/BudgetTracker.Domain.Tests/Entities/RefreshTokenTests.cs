using BudgetTracker.Domain.Entities;
using BudgetTracker.Domain.Common.Exceptions;
using FluentAssertions;
using Xunit;

namespace BudgetTracker.Domain.Tests.Entities;

public class RefreshTokenTests
{
    private const int TestUserId = 1;
    private const string TestToken = "test-refresh-token-12345678901234567890";

    #region Factory Method Tests

    [Fact]
    public void Create_WithValidData_ShouldSucceed()
    {
        // Arrange
        var expiresAt = DateTime.UtcNow.AddDays(7);
        var ipAddress = "192.168.1.1";

        // Act
        var token = RefreshToken.Create(TestUserId, TestToken, expiresAt, ipAddress);

        // Assert
        token.Should().NotBeNull();
        token.Token.Should().Be(TestToken);
        token.UserId.Should().Be(TestUserId);
        token.ExpiresAt.Should().Be(expiresAt);
        token.CreatedByIp.Should().Be(ipAddress);
        token.IsRevoked.Should().BeFalse();
        token.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Create_WithEmptyToken_ShouldThrowDomainException()
    {
        // Arrange
        var expiresAt = DateTime.UtcNow.AddDays(7);

        // Act
        var act = () => RefreshToken.Create(TestUserId, "", expiresAt);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*Token cannot be empty*");
    }

    [Fact]
    public void Create_WithShortToken_ShouldThrowDomainException()
    {
        // Arrange
        var expiresAt = DateTime.UtcNow.AddDays(7);
        var shortToken = "short";

        // Act
        var act = () => RefreshToken.Create(TestUserId, shortToken, expiresAt);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*Token is too short*");
    }

    [Fact]
    public void Create_WithPastExpirationDate_ShouldThrowDomainException()
    {
        // Arrange
        var pastDate = DateTime.UtcNow.AddDays(-1);

        // Act
        var act = () => RefreshToken.Create(TestUserId, TestToken, pastDate);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*expiration date must be in the future*");
    }

    #endregion

    #region IsExpired Tests

    [Fact]
    public void IsExpired_WhenExpiresAtIsInPast_ShouldReturnTrue()
    {
        // Arrange
        var expiresAt = DateTime.UtcNow.AddDays(-1);
        var token = RefreshToken.Create(TestUserId, TestToken, expiresAt.AddDays(2));

        SetPrivateProperty(token, nameof(RefreshToken.ExpiresAt), expiresAt);

        // Act
        var result = token.IsExpired;

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsExpired_WhenExpiresAtIsInFuture_ShouldReturnFalse()
    {
        // Arrange
        var expiresAt = DateTime.UtcNow.AddDays(7);
        var token = RefreshToken.Create(TestUserId, TestToken, expiresAt);

        // Act
        var result = token.IsExpired;

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsExpired_WhenExpiresAtIsNow_ShouldReturnTrue()
    {
        // Arrange
        var expiresAt = DateTime.UtcNow;
        var token = RefreshToken.Create(TestUserId, TestToken, expiresAt.AddSeconds(1));
        SetPrivateProperty(token, nameof(RefreshToken.ExpiresAt), expiresAt);

        // Act
        var result = token.IsExpired;

        // Assert
        result.Should().BeTrue();
    }

    #endregion

    #region IsRevoked Tests

    [Fact]
    public void IsRevoked_WhenNotRevoked_ShouldReturnFalse()
    {
        // Arrange
        var expiresAt = DateTime.UtcNow.AddDays(7);
        var token = RefreshToken.Create(TestUserId, TestToken, expiresAt);

        // Act
        var result = token.IsRevoked;

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsRevoked_WhenRevoked_ShouldReturnTrue()
    {
        // Arrange
        var expiresAt = DateTime.UtcNow.AddDays(7);
        var token = RefreshToken.Create(TestUserId, TestToken, expiresAt);

        // Act
        token.Revoke();

        // Assert
        token.IsRevoked.Should().BeTrue();
        token.RevokedAt.Should().NotBeNull();
    }

    #endregion

    #region IsActive Tests

    [Fact]
    public void IsActive_WhenNotExpiredAndNotRevoked_ShouldReturnTrue()
    {
        // Arrange
        var expiresAt = DateTime.UtcNow.AddDays(7);
        var token = RefreshToken.Create(TestUserId, TestToken, expiresAt);

        // Act
        var result = token.IsActive;

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsActive_WhenExpired_ShouldReturnFalse()
    {
        // Arrange
        var expiresAt = DateTime.UtcNow.AddDays(-1);
        var token = RefreshToken.Create(TestUserId, TestToken, DateTime.UtcNow.AddDays(1)); 
        SetPrivateProperty(token, nameof(RefreshToken.ExpiresAt), expiresAt);

        // Act
        var result = token.IsActive;

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsActive_WhenRevoked_ShouldReturnFalse()
    {
        // Arrange
        var expiresAt = DateTime.UtcNow.AddDays(7);
        var token = RefreshToken.Create(TestUserId, TestToken, expiresAt);
        token.Revoke();

        // Act
        var result = token.IsActive;

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsActive_WhenExpiredAndRevoked_ShouldReturnFalse()
    {
        // Arrange
        var expiresAt = DateTime.UtcNow.AddDays(-1);
        var token = RefreshToken.Create(TestUserId, TestToken, DateTime.UtcNow.AddDays(1));
        token.Revoke();
        SetPrivateProperty(token, nameof(RefreshToken.ExpiresAt), expiresAt);

        // Act
        var result = token.IsActive;

        // Assert
        result.Should().BeFalse();
    }

    #endregion

    #region Revoke Tests

    [Fact]
    public void Revoke_ShouldSetRevokedAt()
    {
        // Arrange
        var expiresAt = DateTime.UtcNow.AddDays(7);
        var token = RefreshToken.Create(TestUserId, TestToken, expiresAt);
        var beforeRevoke = DateTime.UtcNow;

        // Act
        token.Revoke();

        // Assert
        var afterRevoke = DateTime.UtcNow;
        token.RevokedAt.Should().NotBeNull();
        token.RevokedAt.Should().BeOnOrAfter(beforeRevoke);
        token.RevokedAt.Should().BeOnOrBefore(afterRevoke);
    }

    [Fact]
    public void Revoke_WithIpAddress_ShouldSetRevokedByIp()
    {
        // Arrange
        var expiresAt = DateTime.UtcNow.AddDays(7);
        var token = RefreshToken.Create(TestUserId, TestToken, expiresAt);
        var ipAddress = "192.168.1.1";

        // Act
        token.Revoke(revokedByIp: ipAddress);

        // Assert
        token.RevokedByIp.Should().Be(ipAddress);
    }

    [Fact]
    public void Revoke_WithReplacementToken_ShouldSetReplacedByToken()
    {
        // Arrange
        var expiresAt = DateTime.UtcNow.AddDays(7);
        var token = RefreshToken.Create(TestUserId, TestToken, expiresAt);
        var newToken = "new-token-12345678901234567890";

        // Act
        token.Revoke(replacedByToken: newToken);

        // Assert
        token.ReplacedByToken.Should().Be(newToken);
    }

    [Fact]
    public void Revoke_WhenAlreadyRevoked_ShouldThrowDomainException()
    {
        // Arrange
        var expiresAt = DateTime.UtcNow.AddDays(7);
        var token = RefreshToken.Create(TestUserId, TestToken, expiresAt);
        token.Revoke();

        // Act
        var act = () => token.Revoke();

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*already revoked*");
    }

    #endregion

    #region ValidateForRefresh Tests

    [Fact]
    public void ValidateForRefresh_WhenTokenIsActive_ShouldNotThrow()
    {
        // Arrange
        var expiresAt = DateTime.UtcNow.AddDays(7);
        var token = RefreshToken.Create(TestUserId, TestToken, expiresAt);

        // Act
        var act = () => token.ValidateForRefresh();

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void ValidateForRefresh_WhenTokenIsExpired_ShouldThrowDomainException()
    {
        // Arrange
        var expiresAt = DateTime.UtcNow.AddDays(-1);
        var token = RefreshToken.Create(TestUserId, TestToken, DateTime.UtcNow.AddDays(1));
        SetPrivateProperty(token, nameof(RefreshToken.ExpiresAt), expiresAt);

        // Act
        var act = () => token.ValidateForRefresh();

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*expired*")
            .Where(e => e.ErrorCode == "TOKEN_EXPIRED");
    }

    [Fact]
    public void ValidateForRefresh_WhenTokenIsRevoked_ShouldThrowDomainException()
    {
        // Arrange
        var expiresAt = DateTime.UtcNow.AddDays(7);
        var token = RefreshToken.Create(TestUserId, TestToken, expiresAt);
        token.Revoke();

        // Act
        var act = () => token.ValidateForRefresh();

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*revoked*")
            .Where(e => e.ErrorCode == "TOKEN_REVOKED");
    }

    #endregion

    #region IsExpiringSoon Tests

    [Fact]
    public void IsExpiringSoon_WhenExpiresWithinThreshold_ShouldReturnTrue()
    {
        // Arrange
        var expiresAt = DateTime.UtcNow.AddHours(12);
        var token = RefreshToken.Create(TestUserId, TestToken, expiresAt);
        var threshold = TimeSpan.FromDays(1);

        // Act
        var result = token.IsExpiringSoon(threshold);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsExpiringSoon_WhenExpiresAfterThreshold_ShouldReturnFalse()
    {
        // Arrange
        var expiresAt = DateTime.UtcNow.AddDays(7);
        var token = RefreshToken.Create(TestUserId, TestToken, expiresAt);
        var threshold = TimeSpan.FromDays(1);

        // Act
        var result = token.IsExpiringSoon(threshold);

        // Assert
        result.Should().BeFalse();
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Helper do ustawienia private property w testach (reflection).
    /// Używany TYLKO w testach, gdy musisz zmockować stan dla edge cases.
    /// </summary>
    private static void SetPrivateProperty<T>(object obj, string propertyName, T value)
    {
        var property = obj.GetType().GetProperty(propertyName);
        if (property != null)
        {
            property.SetValue(obj, value);
        }
    }

    #endregion
}