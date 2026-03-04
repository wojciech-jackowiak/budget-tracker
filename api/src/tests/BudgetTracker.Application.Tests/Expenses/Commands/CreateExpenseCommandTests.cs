using BudgetTracker.Application.Common.Interfaces;
using BudgetTracker.Application.Expenses.Commands.CreateExpense;
using BudgetTracker.Domain.Entities;
using BudgetTracker.Domain.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetTracker.Application.Tests.Expenses.Commands
{
    public class CreateExpenseCommandTests
    {
        #region Setup
        private readonly Mock<IBudgetTrackerDbContext> _contextMock;
        private readonly CreateExpenseHandler _handler;
        private readonly List<Transaction> _transactions;
        private readonly List<Category> _categories;

        public CreateExpenseCommandTests()
        {
            _contextMock = new Mock<IBudgetTrackerDbContext>();
            _transactions = new List<Transaction>();
            _categories = new List<Category>();

            SetupCategoriesDbSet();
            SetupTransactionsDbSet();

            _handler = new CreateExpenseHandler(_contextMock.Object);
        }

        private void SetupCategoriesDbSet()
        {
            var testCategory = Category.CreateSystem("Test Category", "📝", "#999999");
            typeof(Category).GetProperty("Id")!.SetValue(testCategory, 1);
            _categories.Add(testCategory);

            var mockCategoriesSet = _categories.BuildMockDbSet();
            _contextMock
                .Setup(x => x.Categories)
                .Returns(mockCategoriesSet.Object);
        }

        private void SetupTransactionsDbSet()
        {
            var mockSet = new Mock<DbSet<Transaction>>();

            mockSet
                .Setup(m => m.Add(It.IsAny<Transaction>()))
                .Callback<Transaction>(t =>
                {
                    var currentId = _transactions.Count + 1;
                    typeof(Transaction).GetProperty("Id")!.SetValue(t, currentId);
                    _transactions.Add(t);
                });

            _contextMock
                .Setup(x => x.Transactions)
                .Returns(mockSet.Object);

            _contextMock
                .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);
        }
        #endregion


        [Fact]
        public async Task Handle_ValidCommand_ShouldReturnNewTransactionId()
        {
            // Arrange
            var command = new CreateExpenseCommand
            {
                Amount = 100.50m,
                Description = "Grocery shopping",
                Date = DateTime.UtcNow,
                CategoryId = 1,
                UserId = 1
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task Handle_ValidCommand_ShouldCreateTransactionWithCorrectData()
        {
            // Arrange
            var command = new CreateExpenseCommand
            {
                Amount = 100.50m,
                Description = "Grocery",
                Date = new DateTime(2025, 12, 10, 12, 0, 0, DateTimeKind.Utc),
                CategoryId = 1,
                UserId = 1
            };

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _transactions.Should().HaveCount(1);

            var created = _transactions[0];
            created.Amount.Should().Be(100.50m);
            created.Description.Should().Be("Grocery");
            created.CategoryId.Should().Be(1);
            created.UserId.Should().Be(1);
            created.Type.Should().Be(TransactionType.Expense);
            created.MonthYear.Should().Be("2025-12"); 
        }

        [Fact]
        public async Task Handle_ValidCommand_ShouldSetTransactionTypeToExpense()
        {
            // Arrange
            var command = new CreateExpenseCommand
            {
                Amount = 50m,
                Description = "Coffee",
                Date = DateTime.UtcNow,
                CategoryId = 1,
                UserId = 1
            };

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _transactions[0].Type.Should().Be(TransactionType.Expense);
        }

        [Fact]
        public async Task Handle_ValidCommand_ShouldCallSaveChangesAsync()
        {
            // Arrange
            var command = new CreateExpenseCommand
            {
                Amount = 25m,
                Description = "Bus ticket",
                Date = DateTime.UtcNow,
                CategoryId = 1,
                UserId = 1
            };

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _contextMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_WithNonExistentCategory_ShouldThrowNotFoundException()
        {
            // Arrange
            var command = new CreateExpenseCommand
            {
                Amount = 15m,
                Description = "Random expense",
                Date = DateTime.UtcNow,
                CategoryId = 999, 
                UserId = 1
            };

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<BudgetTracker.Application.Common.Exceptions.NotFoundException>();
        }

        [Fact]
        public async Task Handle_ShouldSetMonthYearAutomatically()
        {
            // Arrange
            var testDate = new DateTime(2026, 1, 15);
            var command = new CreateExpenseCommand
            {
                Amount = 50m,
                Description = "Test",
                Date = testDate,
                CategoryId = 1,
                UserId = 1
            };

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _transactions[0].MonthYear.Should().Be("2026-01");
        }
    }
}
