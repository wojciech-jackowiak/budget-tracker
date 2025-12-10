using BudgetTracker.Domain.Common;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetTracker.Domain.Tests.Common
{
    public class BaseEntityTests
    {
        private class TestEntity : BaseEntity { }
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

        #region UpdatedAt  Tests
        [Fact]
        public void NewEntity_ShouldHaveUpdatedAtNull()
        {
            // Arrange & Act
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

        #endregion
    }
}
