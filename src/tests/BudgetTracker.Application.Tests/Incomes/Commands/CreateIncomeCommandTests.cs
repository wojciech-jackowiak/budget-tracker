using BudgetTracker.Application.Common.Interfaces;
using BudgetTracker.Application.Expenses.Commands.CreateExpense;
using BudgetTracker.Application.Expenses.Commands.CreateIncome;
using BudgetTracker.Domain.Entities;
using BudgetTracker.Domain.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetTracker.Application.Tests.Incomes.Commands
{
    public class CreateIncomeCommandTests
    {
        private readonly Mock<IBudgetTrackerDbContext> _contextMock;
        private readonly CreateIncomeCommandHandler _handler;
        private readonly List<Transaction> _transactions;
        private int _idCounter = 1;
        public CreateIncomeCommandTests()
        {
            _contextMock = new Mock<IBudgetTrackerDbContext>();
            _transactions = new List<Transaction>();

            SetupTransactionsDbSet();

            _handler = new CreateIncomeCommandHandler(_contextMock.Object);
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
            var command = new CreateIncomeCommand
            {
                Amount = 5000,
                Description = "Monthly salary",
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
            var command = new CreateIncomeCommand
            {
                Amount = 500,
                Description = "Deposit",
                Date = new DateTime(2025, 12, 10, 12, 0, 0, DateTimeKind.Utc),
                CategoryId = 1,
                UserId = 1
            };

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _transactions.Should().HaveCount(1);

            var created = _transactions[0];
            created.Amount.Should().Be(500);
            created.Description.Should().Be("Deposit");
            created.CategoryId.Should().Be(1);
            created.UserId.Should().Be(1);
        }
        [Fact]
        public async Task Handle_ValidCommand_ShouldSetTransactionTypeToIncome()
        {
            // Arrange
            var command = new CreateIncomeCommand
            {
                Amount = 250,
                Description = "Tax return",
                Date = DateTime.UtcNow,
                UserId = 1
            };

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _transactions[0].Type.Should().Be(TransactionType.Income);
        }
       
        [Fact]
        public async Task Handle_ValidCommand_ShouldCallSaveChangesAsync()
        {
            // Arrange
            var command = new CreateIncomeCommand
            {
                Amount = 80,
                Description = "Winning on lottery",
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
            var command = new CreateIncomeCommand
            {
                Amount = 500,
                Description = "Extra working hours",
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
