using BudgetTracker.Domain.Common;
using FluentAssertions;
using Xunit;

namespace BudgetTracker.Domain.Tests.Common;

public class BaseEntityTests
{
    private class TestEntity : BaseEntity
    {
        public new int Id { get => base.Id; set => base.Id = value; }
    }

    #region CreatedAt Tests

    [Fact]
    public void NewEntity_ShouldHaveCreatedAtSetAutomatically()
    {
        // Arrange
        var before = DateTime.UtcNow;

        // Act
        var entity = new TestEntity();

        // Assert
        var after = DateTime.UtcNow;
        entity.CreatedAt.Should().BeOnOrAfter(before);
        entity.CreatedAt.Should().BeOnOrBefore(after);
    }

    #endregion

    #region UpdatedAt Tests

    [Fact]
    public void NewEntity_ShouldHaveUpdatedAtNull()
    {
        var entity = new TestEntity();

        // Assert
        entity.UpdatedAt.Should().BeNull();
    }

    [Fact]
    public void MarkAsModified_ShouldSetUpdatedAt()
    {
        // Arrange
        var entity = new TestEntity();
        var before = DateTime.UtcNow;

        // Act
        entity.MarkAsModified();

        // Assert
        var after = DateTime.UtcNow;
        entity.UpdatedAt.Should().NotBeNull();
        entity.UpdatedAt.Should().BeOnOrAfter(before);
        entity.UpdatedAt.Should().BeOnOrBefore(after);
    }

    [Fact]
    public void MarkAsModified_CalledMultipleTimes_ShouldUpdateUpdatedAt()
    {
        // Arrange
        var entity = new TestEntity();
        entity.MarkAsModified();
        var firstUpdate = entity.UpdatedAt;

        // Wait a bit to ensure different timestamp
        System.Threading.Thread.Sleep(10);

        // Act
        entity.MarkAsModified();

        // Assert
        entity.UpdatedAt.Should().NotBeNull();
        entity.UpdatedAt.Should().BeAfter(firstUpdate!.Value);
    }

    #endregion

    #region IsNew Tests

    [Fact]
    public void IsNew_WhenIdIsZero_ShouldReturnTrue()
    {
        // Arrange
        var entity = new TestEntity { Id = 0 };

        // Act
        var result = entity.IsNew;

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsNew_WhenIdIsGreaterThanZero_ShouldReturnFalse()
    {
        // Arrange
        var entity = new TestEntity { Id = 1 };

        // Act
        var result = entity.IsNew;

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsNew_WhenNewlyCreated_ShouldReturnTrue()
    {
        // Arrange & Act
        var entity = new TestEntity();

        // Assert
        entity.IsNew.Should().BeTrue();
        entity.Id.Should().Be(0);
    }

    #endregion
}