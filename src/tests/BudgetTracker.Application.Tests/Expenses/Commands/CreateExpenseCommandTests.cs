using BudgetTracker.Application.Common.Interfaces;
using BudgetTracker.Application.Expenses.Commands.CreateExpense;
using BudgetTracker.Domain.Entities;
using BudgetTracker.Domain.Enums;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace BudgetTracker.Application.Tests.Expenses.Commands
{
    public class CreateExpenseCommandTests
    {
        private readonly Mock<IBudgetTrackerDbContext> _contextMock;
        private readonly CreateExpenseCommandHandler _handler;
        private readonly List<Transaction> _transactions;
        private int _idCounter = 1;

        public CreateExpenseCommandTests()
        {
            _contextMock = new Mock<IBudgetTrackerDbContext>();
            _transactions = new List<Transaction>();

            SetupTransactionsDbSet();

            _handler = new CreateExpenseCommandHandler(_contextMock.Object);
        }

        private void SetupTransactionsDbSet()
        {
            var mockSet = new Mock<DbSet<Transaction>>();
            
            mockSet
                .Setup(m => m.Add(It.IsAny<Transaction>()))
                .Callback<Transaction>(t =>
                {
                    t.Id = _idCounter++;
                    _transactions.Add(t);
                });

            _contextMock
                .Setup(x => x.Transactions)
                .Returns(mockSet.Object);

            _contextMock
                .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);
        }

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
        public async Task Handle_WithoutCategoryId_ShouldCreateTransactionWithNullCategory()
        {
            // Arrange
            var command = new CreateExpenseCommand
            {
                Amount = 15m,
                Description = "Random expense",
                Date = DateTime.UtcNow,
                CategoryId = null,
                UserId = 1
            };

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _transactions[0].CategoryId.Should().BeNull();
        }
    }
}
